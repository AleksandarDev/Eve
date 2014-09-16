/// <reference path="../../Scripts/jquery.d.ts" />
/// <reference path="../../Scripts/chrome.d.ts" />

class RunAt {
	static Start: string = "document_start";
	static End: string = "document_end";
	static Idle: string = "document_idle";
}

class Background {	
	private webSocketPort: string = "41258";
	private webSocketHost: string = "ws://localhost";
	private webSocket: WebSocket;


	constructor() {
		this.InitializeWebSockets(); 
	}
	
	private InitializeWebSockets(): void {
		// Attach port and create new web socket
		this.webSocketHost += ":" + this.webSocketPort;
		this.webSocket = new WebSocket(this.webSocketHost);
		
		// Web socket handling methods
		this.webSocket.onmessage = this.WebSocketOnMessage.bind(this);
		this.webSocket.onopen = this.WebSocketOnOpen.bind(this);
		this.webSocket.onclose = this.WebSocketOnClose.bind(this);
		this.webSocket.onerror = this.WebSocketOnError.bind(this);
	}

	private WebSocketOnMessage(message: MessageEvent): void {
		try {
			console.log("Got message: ");
			console.log(message);

			chrome.tabs.query({}, (tabs: chrome.tabs.Tab[]) => {
				for(var index = 0; index < tabs.length; index++)
					this.ExecuteScript(tabs[index].id, message.data);
			});
		} catch (exception) {
			console.warn(exception);
		}
	}

	private WebSocketOnOpen(event: Event): void {
		console.log("Socket opened to [" + this.webSocketHost + "]");
	}

	private WebSocketOnClose(event: CloseEvent): void {
		console.log("Socket closed!");
	}

	private WebSocketOnError(evet: ErrorEvent): void {
		console.error(event);
	}

	// Executes give script code on specific tab
	// Script can be injected into all frames by seting deepInject to true
	// Script by default doesn't run before document finished loading DOM, this can be changes using runAt parameter
	ExecuteScript(tabID: number, code: string, deepInject: bool = false, runAt: string = RunAt.End): void {
		// Begin script execution
		chrome.tabs.executeScript(tabID,
			{
				code: "var scriptToInject = document.createElement('script'); scriptToInject.type = 'text/javascript'; scriptToInject.innerHTML = '" + code + "'; document.getElementsByTagName('head')[0].appendChild(scriptToInject);",
				allFrames: deepInject,
				runAt: "document_end"
			},
			this.ScriptExecutionCallback);
	}

	// Method that handles result of script execution
	private ScriptExecutionCallback(result: any): void {
		console.log("Script executed!");
		if (result !== undefined) {
			console.log("Script results: ");
			console.log(result);
		}
	}
}

// Create an instance of Background 
var background: Background = new Background();