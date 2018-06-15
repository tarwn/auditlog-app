import validation from '../extenders/validation';
import ViewHeaderLinkModel from './viewHeaderLinkModel';

class ViewColumnConfigurationModel {
    constructor(rawData) {
        this.order = ko.observable(rawData.order);
        this.label = ko.observable(rawData.label).extend(validation.string(true, 40));
        this.type = ko.observable(rawData.type);

        if (rawData.type === 'multiline') {
            this.lines = [
                {
                    type: ko.observable(rawData.lines[0].type),
                    field: ko.observable(rawData.lines[0].field)
                },
                {
                    type: ko.observable(rawData.lines[1].type),
                    field: ko.observable(rawData.lines[1].field)
                }
            ];
        }
        else {
            this.field = ko.observable(rawData.field);
        }
    }
}

class ViewConfigurationModel {
    constructor(rawData) {
        this.id = rawData.id;
        this.key = ko.observable(rawData.key);

        this.custom = {
            url: ko.observable(rawData.custom.url).extend(validation.url(false, 400)),
            logo: ko.observable(rawData.custom.logo).extend(validation.url(false, 400)),
            title: ko.observable(rawData.custom.title).extend(validation.string(false, 40)),
            headerLinks: ko.observableArray(rawData.custom.headerLinks.map((link) => {
                return new ViewHeaderLinkModel(link, true);
            })),
            copyright: ko.observable(rawData.custom.copyright).extend(validation.string(false, 80))
        };

        this.columns = ko.observableArray(rawData.columns.map((col) => {
            return new ViewColumnConfigurationModel(col);
        }));

        this.isValid = ko.pureComputed(() => this.custom.url.isValid() &&
                this.custom.logo.isValid() &&
                this.custom.title.isValid() &&
                this.custom.copyright.isValid() &&
                this.custom.headerLinks().every(hl => hl.isValid()));
    }

    addHeaderLink(newLink) {
        this.custom.headerLinks.push(newLink);
    }

    removeHeaderLink(linkToRemove) {
        this.custom.headerLinks.remove(link => link.url === linkToRemove.url &&
                                               link.label === linkToRemove.label);
    }
}

export default ViewConfigurationModel;
