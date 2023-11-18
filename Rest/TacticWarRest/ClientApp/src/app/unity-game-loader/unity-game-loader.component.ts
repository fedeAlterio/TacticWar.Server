import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'unity-game-loader',
  templateUrl: './unity-game-loader.component.html',
  styleUrls: ['./unity-game-loader.component.scss']
})
export class UnityGameLoaderComponent implements OnInit {
  async ngOnInit(): Promise<void>  {
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

    let container = document.querySelector("#unity-container") || new Element();
    var canvas: HTMLElement = document.querySelector("#unity-canvas") || new HTMLElement();
    var loadingBar: HTMLElement = document.querySelector("#unity-loading-bar") || new HTMLElement();
    var progressBarFull: HTMLElement = document.querySelector("#unity-progress-bar-full") || new HTMLElement();
    var fullscreenButton: HTMLElement = document.querySelector("#unity-fullscreen-button") || new HTMLElement();
    const unityInstance = await createUnityInstance(canvas, config);
    fullscreenButton.onclick = () => {
      unityInstance.SetFullscreen(1);
    };  
  }

}