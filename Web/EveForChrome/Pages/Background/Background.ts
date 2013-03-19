// Declare Third-Party script classes as dynamic
declare var $: any;
declare var chrome: any;

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

	private WebSocketOnMessage(event: any): void {
		try {
			console.log("Got message: ");
			console.log(event);

			if (event.data == "grooveshark:next-song") {
				chrome.tabs.query({ url: "http://*.grooveshark.com/*" }, (tab: any) => {
					this.ExecuteScript(tab[0].id, '$("#play-next").click();');
				});
			}
			else if (event.data == "grooveshark:prev-song") {
				chrome.tabs.query({ url: "http://*.grooveshark.com/*" }, (tab: any) => {
					this.ExecuteScript(tab[0].id, '$("#play-prev").click();');
				});
			}
			else if (event.data == "grooveshark:pause-song") {
				chrome.tabs.query({ url: "http://*.grooveshark.com/*" }, (tab: any) => {
					this.ExecuteScript(tab[0].id, 'if($("#play-pause").hasClass("playing")) $("#play-pause").click();');
				});
			}
			else if (event.data == "grooveshark:resume-song") {
				chrome.tabs.query({ url: "http://*.grooveshark.com/*" }, (tab: any) => {
					this.ExecuteScript(tab[0].id, 'if(!$("#play-pause").hasClass("playing")) $("#play-pause").click();');
				});
			}
		} catch (exception) {
			console.warn(exception);
		}
	}

	private WebSocketOnOpen(event: any): void {
		console.log("Socket opened to [" + this.webSocketHost + "]");
	}

	private WebSocketOnClose(event: any): void {
		console.log("Socket closed!");
	}

	private WebSocketOnError(evet: any): void {
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