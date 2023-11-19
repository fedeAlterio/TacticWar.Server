import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, WritableSignal, signal } from '@angular/core';
import { from, fromEvent, map, merge, Observable, startWith, Subscription } from "rxjs";

@Component({
  selector: 'unity-game-loader',
  templateUrl: './unity-game-loader.component.html',
  styleUrls: ['./unity-game-loader.component.scss']
})
export class UnityGameLoaderComponent implements OnInit, AfterViewInit {

  width: WritableSignal<number>;
  height: WritableSignal<number>;

  constructor(){
    this.width = signal(0);
    this.height = signal(0);
  }

  @ViewChild('root')
  rootDiv: ElementRef = null!;

  async ngOnInit(): Promise<void> {
    signal(0)
    var canvas: HTMLElement = document.querySelector("#unity-canvas") || new HTMLElement();
    await this.loadGame(canvas);
  }

  ngAfterViewInit(): void {
      const resizeEvent = fromEvent(window, 'resize');
      const fullscreenEvent = fromEvent(window, 'fullscreenchange');
      merge(fullscreenEvent, resizeEvent)
      .pipe(     
        map(_ => undefined),
        startWith(undefined)
      ).subscribe(_ => {
        var width = this.rootDiv.nativeElement.offsetWidth;
        var height = this.rootDiv.nativeElement.offsetHeight - 50;
        this.width.set(width);
        this.height.set(height);
      })
  }

  async loadGame(canvas: HTMLElement): Promise<void> {
    var buildUrl = "assets/tactic-war-unity/Build";
    var loaderUrl = buildUrl + "/tacticwar.loader.js";
    var config = {
      dataUrl: buildUrl + "/tacticwar.data.unityweb",
      frameworkUrl: buildUrl + "/tacticwar.framework.js.unityweb",
      codeUrl: buildUrl + "/tacticwar.wasm.unityweb",
      streamingAssetsUrl: "StreamingAssets",
      companyName: "DefaultCompany",
      productName: "TacticWar",
      productVersion: "0.1",
    };

    var fullscreenButton: HTMLElement = document.querySelector("#unity-fullscreen-button") || new HTMLElement();
    const unityInstance = await createUnityInstance(canvas, config);
    fullscreenButton.onclick = () => {
      unityInstance.SetFullscreen(1);
    };
  }
}
