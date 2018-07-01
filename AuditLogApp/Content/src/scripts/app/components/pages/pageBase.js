export default class PageBase {
    constructor(params) {
        this._services = params.services;
        this._sitewideContext = params.sitewideContext;
        this._navigationContext = params.navigationContext;
        this._subscriptions = [];

        this.readyForDisplay = ko.observable(false);
    }

    initialize() {
        this.readyForDisplay(true);
    }

    dispose() {
        this._subscriptions.forEach((s) => {
            s.dispose();
        });
    }
}
