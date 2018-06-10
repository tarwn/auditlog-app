class ViewHeaderLinkModel {
    constructor(rawData) {
        this.label = ko.observable(rawData.label);
        this.url = ko.observable(rawData.url);
    }
}

class ViewColumnConfigurationModel {
    constructor(rawData) {
        this.order = ko.observable(rawData.order);
        this.label = ko.observable(rawData.label);
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

        this.custom = {};
        this.custom.url = ko.observable(rawData.custom.url);
        this.custom.logo = ko.observable(rawData.custom.logo);
        this.custom.title = ko.observable(rawData.custom.url);
        this.custom.headerLinks = ko.observableArray(rawData.custom.headerLinks.map((link) => {
            return new ViewHeaderLinkModel(link);
        }));
        this.custom.copyright = ko.observable(rawData.custom.copyright);

        this.columns = ko.observableArray(rawData.columns.map((col) => {
            return new ViewColumnConfigurationModel(col);
        }));
    }
}

export default ViewConfigurationModel;
