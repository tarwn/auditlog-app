export default {
    name: 'page-home',
    viewModel: class HomePage {
        constructor(params) {
            this._services = params.services;
            this._sitewideContext = params.sitewideContext;
            this._navigationContext = params.navigationContext;
            this.readyForDisplay = params.readyForDisplay;
            this.params = params;

            this.initialize();
        }

        initialize() {
            setTimeout(() => {
                this.readyForDisplay(true);
            }, 3000);
        }

        // dispose() {
        // }
    },
    template: `
        stuff
    `
};
