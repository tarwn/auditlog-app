import allComponents from '../components/allComponents';
import validation from '../extenders/validation';
import applyHightlightBinding from '../bindings/highlight';

import applyBindingDetailsClick from '../borrowed/auditLogDropIn/utils/applyBindingDetailsClick';
import applyBindingPotentialLink from '../borrowed/auditLogDropIn/utils/applyBindingPotentialLink';

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
        validation.applyExtender(ko);

        // COPIED: dropin/app.js, ln 26
        applyBindingDetailsClick(ko, $);
        applyBindingPotentialLink(ko, $);
        applyHightlightBinding(ko);

        allComponents.forEach((component) => {
            if (component.viewModel === undefined) {
                if (component.viewmodel !== undefined) {
                    throw new Error(`${component.name} component's viewModel is capitalized incorrectly: currently viewmodel (capitalize the M!)`);
                }
                else {
                    throw new Error(`${component.name} component's viewModel is missing!`);
                }
            }

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
