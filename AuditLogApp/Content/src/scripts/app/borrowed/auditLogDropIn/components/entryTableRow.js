
const MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
function padTwo(value) {
    if (value.length === 2) {
        return value;
    }
    else if (value.length === 1) {
        return `0${value}`;
    }
    else {
        return '00';
    }
}

export default class EntryTableRow {
    constructor(rawRow, columns) {
        this.id = rawRow.id;
        this.columns = columns.map(c => ({
            order: c.order,
            type: c.type,
            value: EntryTableRow.createRowValue(c, rawRow)
        }));

        this.row = rawRow;
        this.row.display = {
            time: new Date(rawRow.time).toUTCString(),
            receivedAt: new Date(rawRow.receptionTime).toUTCString(),
            receivedLatency: EntryTableRow._formatLatency(rawRow.receptionTime, rawRow.time)
        };
    }

    static createRowValue(column, rowData) {
        if (column.hasMultipleRows) {
            return EntryTableRow._createMultiLineValue(column, rowData);
        }
        else {
            return EntryTableRow._getFormattedValue(column.lines[0], rowData);
        }
    }

    static _createMultiLineValue(column, rowData) {
        const firstLine = EntryTableRow._getFormattedValue(column.lines[0], rowData);
        if (firstLine == null) {
            return '';
        }
        const secondLine = EntryTableRow._getFormattedValue(column.lines[1], rowData) || '';
        return `<div class="aldi-row-multi-1">${firstLine}</div><div class="aldi-row-multi-2">${secondLine}</div>`;
    }

    static compare(columnName, entryA, entryB) {
        const a = entryA.row;
        const b = entryB.row;

        switch (columnName) {
            case 'id':
            case 'receptionTime':
            case 'time':
            case 'action':
            case 'description':
            case 'url':
                return (a[columnName] || '') > (b[columnName] || '');
            case 'actor.uuid':
                return (a.actor.uuid || '') > (b.actor.uuid || '');
            case 'actor.name':
                return (a.actor.name || '') > (b.actor.name || '');
            case 'actor.email':
                return (a.actor.email || '').toLowerCase() > (b.actor.email || '').toLowerCase();
            case 'context.client.ipAddress':
                return (a.context.client.ipAddress || '') > (b.context.client.ipAddress || '');
            case 'context.client.browserAgent':
                return (a.context.client.browserAgent || '') > (b.context.client.browserAgent || '');
            case 'context.server.server':
                return (a.context.server.server || '') > (b.context.server.server || '');
            case 'context.server.version':
                return (a.context.server.version || '') > (b.context.server.version || '');
            case 'target.type':
                return (a.target.type || '') > (b.target.type || '');
            case 'target.uuid':
                return (a.target.uuid || '') > (b.target.uuid || '');
            case 'target.label':
                return (a.target.label || '') > (b.target.label || '');
            case 'target.url':
                return (a.target.url || '') > (b.target.url || '');
            default:
                return null;
        }
    }

    static _getValue(columnName, rowData) {
        switch (columnName) {
            case 'id':
            case 'receptionTime':
            case 'time':
            case 'action':
            case 'description':
            case 'url':
                return rowData[columnName];
            case 'actor.uuid':
                return rowData.actor.uuid;
            case 'actor.name':
                return rowData.actor.name;
            case 'actor.email':
                return rowData.actor.email;
            case 'context.client.ipAddress':
                return rowData.context.client.ipAddress;
            case 'context.client.browserAgent':
                return rowData.context.client.browserAgent;
            case 'context.server.server':
                return rowData.context.server.server;
            case 'context.server.version':
                return rowData.context.server.version;
            case 'target.type':
                return rowData.target.type;
            case 'target.uuid':
                return rowData.target.uuid;
            case 'target.label':
                return rowData.target.label;
            case 'target.url':
                return rowData.target.url;
            default:
                return null;
        }
    }

    static _getFormattedValue(fieldDefinition, rowData) {
        const rawValue = EntryTableRow._getValue(fieldDefinition.field, rowData);

        switch (fieldDefinition.format) {
            case 'time':
                return EntryTableRow._formatShortDateTime(new Date(rawValue));
            case 'date':
                return EntryTableRow._formatShortDate(new Date(rawValue));
            case 'diff':
                return EntryTableRow._formatLatency(rawValue, EntryTableRow._getValue('time', rowData));
            case 'anchor':
                return EntryTableRow._formatAnchor(rawValue);
            default:
                return rawValue;
        }
    }

    static _formatShortDate(date) {
        return `${MONTHS[date.getUTCMonth()]} ${date.getUTCDate()},  ${date.getUTCFullYear()}`;
    }

    static _formatShortDateTime(date) {
        return `${MONTHS[date.getUTCMonth()]} ${date.getUTCDate()} ${padTwo(date.getUTCHours().toString())}:${padTwo(date.getUTCMinutes().toString())}:${padTwo(date.getUTCSeconds().toString())}`;
    }

    static _formatLatency(receivedAt, entryTime) {
        const diff = new Date(receivedAt) - new Date(entryTime);

        const seconds = 1000;
        const minutes = seconds * 60;
        const hours = minutes * 60;
        const days = hours * 24;
        const months = days * 30; // close enough

        if (diff < 5 * seconds) {
            return `${Math.floor(1000 * diff) / 1000}ms`;
        }
        else if (diff < 60 * seconds) {
            return `${Math.floor(10 * (diff / seconds)) / 10}s`;
        }
        if (diff < 60 * minutes) {
            return `${Math.floor(10 * (diff / minutes)) / 10} minutes`;
        }
        else if (diff < days) {
            return `${Math.floor(10 * (diff / hours)) / 10} hours`;
        }
        else if (diff < 2 * months) {
            return `${Math.floor(10 * (diff / days)) / 10} days`;
        }

        // yikes, old data
        return `~${Math.floor(10 * (diff / months)) / 10} months`;
    }

    static _formatAnchor(url) {
        return `<a href="${url}" target="_blank">${url}</a>`;
    }
}
