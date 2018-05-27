export default class AppViewModel {
    constructor(services, sitewideContext) {
        this._services = services;
        this._sitewideContext = sitewideContext;
        this.sitewide = sitewideContext;

        this.readyForDisplay = ko.observable(false);
        this.routes = [];

        this.currentPage = {
            title: ko.observable(),
            activeMenuItem: ko.observable(),
            componentName: ko.observable(),
            componentParams: {
                readyForDisplay: ko.observable(false),
                navigationContext: ko.observable(),
                sitewideContext: this._sitewideContext,
                services: this._services
            }
        };
        this.currentPage.readyForDisplay = ko.pureComputed(() => this.readyForDisplay() &&
                this.currentPage.componentParams.readyForDisplay());
    }

    addPage(pageDefinition) {
        this.routes.push(pageDefinition);
    }

    mount() {
        this.routes.forEach((pageDefinition) => {
            ko.components.register(pageDefinition.component.name, pageDefinition.component);

            page(pageDefinition.route, (context) => {
                this.currentPage.activeMenuItem(pageDefinition.activeMenuItem);
                this.currentPage.componentName(null);
                this.currentPage.componentParams.readyForDisplay(false);
                this.currentPage.componentParams.navigationContext(context);
                this.currentPage.componentName(pageDefinition.component.name);
                // will use the title to announce for screen readers
                this.currentPage.title(pageDefinition.title);
            });
        });
        page.start('/');
        this.readyForDisplay(true);
    }
}
