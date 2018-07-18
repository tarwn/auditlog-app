/* eslint-disable */

var loadCounter = 0;
function auditLog(options) {
    init(options);
}

function init(options) {
    if (!options.target) { 
        showError("AuditLog Drop-In not configured: target missing");
    }

    document.getElementById(options.target).innerHTML += getContent();

    DOMReady(function () {
        var baseUrl = options.host ? options.host + "/dropin/" : "";
        var scripts = [
            'underscore-min.js',
            'jquery-3.3.1.min.js',
            'knockout-3.4.2.js',
            'app.min.js'
        ];

        var styles = [
            'app.min.css'
        ];
                
        scripts.forEach(function (s) {
            var script = document.createElement("script");
            script.src = baseUrl + s;
            document.head.appendChild(script);
        });

        styles.forEach(function (s) {
            var style = document.createElement("link");
            style.rel = "stylesheet";
            style.href = baseUrl + s;
            document.head.appendChild(style);
        });

        mountApp(options);
    });
}

function mountApp(options) {
    // 2^14! roughly 24s
    var isStillLoading = (!window.App || !window.$ || !window._ || !window.ko);
    if (isStillLoading && loadCounter < 14) {
        loadCounter++;
        console.log('retry ' + (Math.pow(2, loadCounter)));
        setTimeout(function () { mountApp(options); }, Math.pow(2, loadCounter));
        return;
    }
    else if (isStillLoading) {
        console.error("Could not initialize AuditLog app");
        return;
    }

    if(!options.mode || options.mode === 'production') {
        var app = new window.App(options);
        app.mount('targetElement');
    }
    else if (options.mode === 'demo') {
        var app = new window.App(options);
        app.mount('targetElement');
    }
    else if(options.mode === 'config') {
        var app = new window.App(options);
        var sampleMessageReceiver = app.applySampleOverride();
        window.addEventListener("message", function(evt){
            console.log("event received");

            if(evt.origin !== window.location.origin)
                return;
            sampleMessageReceiver(evt);
        }, false);

        console.log({ message: 'ready', origin: window.location.origin });
        window.parent.postMessage('ready', window.location.origin);
    }
    else {
        showError("AuditLog Drop-In: unrecognized mode in options");
    }
}

function DOMReady(a,b,c){ b=document,c='addEventListener';b[c]?b[c]('DOMContentLoaded',a):window.attachEvent('onload',a) };

function showError() {
    var errorDiv = document.createElement("div");
    errorDiv.className = "aldi-error-panel";
    errorDiv.innerHTML = message;
    document.body.appendChild(errorDiv);
}

function getContent() {
    return `
        <div class="aldi-initializing" data-bind="visible: isInitializing">Loading...</div>
        <!-- ko if: !isInitializing() -->
        <div id="aldi-header-container" data-bind="visible: !isInitializing()" style="display: none">
            <ko-header params="customization: customization"></ko-header>
        </div>
        <div id="aldi-container">
            <div id="aldi-nav-container" data-bind="visible: !isInitializing()" style="display: none">
                <ko-navigation params="auditList: auditList, onNavigate: navigateData.bind($data)"></ko-navigation>
                <span class="aldi-links"><a href="#">Export <i class="icon-table"></i></a></span>
            </div>
            <div id="aldi-table-container" data-bind="visible: !isInitializing()" style="display: none">
                <ko-audit-list params="customization: customization, auditList: auditList"></ko-audit-list>
                <!--<div id="aldi-details-panel" style="display: none">details panel</div>-->
            </div>
            <div id="aldi-banner" data-bind="visible: !isInitializing()" style="display: none">Powered by <a href="https://www.auditlog.co">AuditLog.co</a></div>
        </div>
        <div id="aldi-footer">
            <ko-footer params="customization: customization"></ko-footer>
        </div>
        <!-- /ko -->
        
        <script id="table-head" type="text/html">
            <thead>
                <tr class="aldi-audit-headrow">
                    <th></th>
                    <% _.each(columns(), function(c){ %>
                        <th data-bind="click: applySort.bind($data, c)" class="aldi-table-sortable">
                            <span data-bind="text: c.label"></span>
                            <i data-bind="css: { 'icon-sort': c !== sortColumn(), 'icon-sort-down': c === sortColumn() && sortDirection() === 'desc', 'icon-sort-up': c === sortColumn() && sortDirection() === 'asc'  }"></i>
                        </th>
                    <% }) %>
                </tr>
            </thead>
        </script>
        <script id="table-row" type="text/html">
            <tbody>
                <% _.each(rows(), function(r){ %>
                    <tr class="aldi-audit-row">
                        <td class="aldi-row-doubleheight">
                            <a href="#" title="Show Details" data-bind="detailsClick: '#aldi-details-panel', rowId: r.id, selectRow: $root.selectRow.bind($root), selectedClass: 'aldi-row-selected'"><i class="icon-plus-circled"></i></a>
                        </td>
                        <% _.each(r.columns, function(c){ %>
                            <td data-bind="html: c.value"></td>
                        <% }); %>
                    </tr>
                <% }); %>
            </tbody>
        </script>
    `
}
/* eslint-enable */
