import validation from '../extenders/validation';
import ViewHeaderLinkModel from './viewHeaderLinkModel';
import ViewColumnConfigurationModel from './viewColumnConfigurationModel';

export default class ViewConfigurationModel {
    constructor(rawData) {
        this.id = rawData.id;

        this.accessKey = ko.observable(rawData.accessKey || 'Not set yet');

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
        this.sortedColumns = ko.pureComputed(() => {
            return this.columns.sort((a, b) => {
                return a.order() - b.order();
            })();
        });

        this.isValid = ko.pureComputed(() => this.custom.url.isValid() &&
                this.custom.logo.isValid() &&
                this.custom.title.isValid() &&
                this.custom.copyright.isValid() &&
            this.custom.headerLinks().every(hl => hl.isValid()));

        this.availableFields = [
            'receptionTime[time]',
            'receptionTime[date]',
            'receptionTime[diff]',
            'uuid',
            'client.id',
            'client.name',
            'time[time]',
            'time[date]',
            'action',
            'description',
            'url[anchor]',
            'actor.uuid',
            'actor.name',
            'actor.email',
            'context.client.ipAddress',
            'context.client.browserAgent',
            'context.server.serverId',
            'context.server.version',
            'target.type',
            'target.uuid',
            'target.label',
            'target.url[anchor]'
        ];
    }

    addHeaderLink(newLink) {
        this.custom.headerLinks.push(newLink);
    }

    removeHeaderLink(linkToRemove) {
        this.custom.headerLinks.remove(link => link.url === linkToRemove.url &&
                                               link.label === linkToRemove.label);
    }

    addColumn(newColumn) {
        newColumn.order(this.columns().length);
        this.columns.push(newColumn);
    }


    removeColumn(columnToRemove) {
        this.columns.remove(col => col.label === columnToRemove.label &&
                                   col.order === columnToRemove.order);

        this.columns().forEach((col, ind) => col.order(ind));
    }

    moveColumnUp(columnToMove) {
        if (columnToMove.order() > 0) {
            const colIndex = columnToMove.order();
            const colNewIndex = colIndex - 1;
            this.columns()[colNewIndex].order(colIndex);
            columnToMove.order(colNewIndex);
        }
    }

    moveColumnDown(columnToMove) {
        if (columnToMove.order() < this.columns().length - 1) {
            const colIndex = columnToMove.order();
            const colNewIndex = colIndex + 1;
            this.columns()[colNewIndex].order(colIndex);
            columnToMove.order(colNewIndex);
        }
    }

    toRawData() {
        // trims down field to match raw data for service calls + passing to iframe
        return {
            id: this.id,
            accessKey: '',
            custom: this.custom,
            columns: this.columns()
        };
    }
}
