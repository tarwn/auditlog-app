import allComponents from '../components/allComponents';

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
                navigationContext: ko.observable(),
                sitewideContext: this._sitewideContext,
                services: this._services
            }
        };
    }

    addPage(pageDefinition) {
        this.routes.push(pageDefinition);
    }

    mount() {
        allComponents.forEach((component) => {
            ko.components.register(component.name, component);
        });

        this.routes.forEach((pageDefinition) => {
            ko.components.register(pageDefinition.component.name, pageDefinition.component);

            page(pageDefinition.route, (context) => {
                this.readyForDisplay(false);
                // now that it's hidden, swap in the new component
                //  rely on new component setting readyForDisplay to true to start displaying
                //  update context just in case? before it's used <- don't like this
                //  swap in menu item and then component, then update title last for announce
                this.currentPage.componentParams.navigationContext(context);
                this.currentPage.activeMenuItem(pageDefinition.activeMenuItem);
                this.currentPage.componentName(pageDefinition.component.name);
                // will use the title to announce for screen readers
                this.currentPage.title(pageDefinition.title);
                this.readyForDisplay(true);
            });
        });
        page.start('/');
        this.readyForDisplay(true);
    }
}
