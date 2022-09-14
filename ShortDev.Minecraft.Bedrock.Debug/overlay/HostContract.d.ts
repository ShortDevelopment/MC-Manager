export { };

declare global {
    interface Window {
        chrome: BrowserInterop;
    }
}

interface BrowserInterop {
    webview: WebView2;
}

interface WebView2 {
    addEventListener(type: 'message', listener: (this: WebView2, e: MessageEvent) => void);
}

interface MessageEvent {
    source: WebView2;
    data: any;
}