import DailyEventCount from './models/dailyEventCount';

export default {
    name: 'daily-event-chart',
    viewModel: class DailyEventChartViewModel {
        constructor(params) {
            this.id = `chart_${Math.round(Math.random() * 10000)}`;
            this.data = params.value.data;

            this.safeData = ko.pureComputed(() => {
                if (this.data() == null || this.data().length === 0) {
                    return [new DailyEventCount(moment().startOf('day'), 0)];
                }
                else {
                    return this.data();
                }
            });

            this.startDate = ko.pureComputed(() => {
                return this.safeData()[0].date;
            });
            this.endDate = ko.pureComputed(() => {
                return this.safeData().slice(-1)[0].date;
            });
            this.eventCountMax = ko.pureComputed(() => {
                return this.data().reduce((acc, d) => Math.max(acc, d.count()), 0);
            });
            this.dataCount = ko.pureComputed(() => {
                return this.safeData().length;
            });

            this.chart = {
                margin: {
                    top: 20,
                    right: 20,
                    bottom: 20,
                    left: 20
                },
                base: ko.observable({
                    width: 600,
                    height: 300
                }),
                dimensions: ko.pureComputed(() => {
                    const { width, height } = this.chart.base();
                    const { margin } = this.chart;
                    return {
                        width: width - margin.left - margin.right,
                        height: height - margin.top - margin.bottom
                    };
                })
            };
            window.addEventListener('resize', () => {
                this._updateChartSize();
            });

            this.subscriptions = [];
            this.subscriptions.push(this.data.subscribe(() => {
                this._refreshRender();
            }));
            this.subscriptions.push(this.chart.dimensions.subscribe(() => {
                this._refreshRender();
            }));


            this._initialRender();
        }

        dispose() {
            this.subscriptions.forEach((s) => {
                s.dispose();
            });
        }

        _initialRender() {
            if (!this._initializeChart()) {
                setTimeout(() => this._initialRender(), 50);
                return;
            }
            this.chart.isInitialized = true;

            this._render();
        }

        _refreshRender() {
            if (!this.chart.isInitialized) {
                return;
            }

            this._render();
        }

        _updateChartSize() {
            const chartDiv = document.getElementById(this.id);
            this.chart.base({
                width: chartDiv.clientWidth,
                height: chartDiv.clientHeight
            });
        }

        _initializeChart() {
            // not visible
            if (d3.select(`#${this.id}`).empty()) {
                return false;
            }

            // wire up real size + resize event
            this._updateChartSize();
            window.addEventListener('resize', () => this._updateChartSize());

            const { width, height } = this.chart.dimensions();
            const { margin } = this.chart;

            // start drawing
            this.chart.svg = d3.select(`#${this.id}`)
                .append('svg')
                .attr('width', width + margin.left + margin.right)
                .attr('height', height + margin.top + margin.bottom);

            // x-axis scale
            this.chart.xScale = d3.scaleBand()
                .range([0, width])
                .round(true)
                .paddingInner(0.1);

            // x-axis drawing
            this.chart.xAxis = d3.axisBottom(this.chart.xScale)
                .ticks(1, 'no data');
            this.chart.svg.append('g')
                .attr('transform', `translate(${margin.left}, ${height + margin.top})`)
                .attr('class', 'x ala-chart-axis')
                .call(this.chart.xAxis);

            // y-axis scale
            // TODO: improve range and tick intervals w/ dynamic rounding
            this.chart.yScale = d3.scaleLinear()
                .range([height, 0])
                .nice();

            // y-axis chart bg
            this.chart.yGrid = d3.axisRight(this.chart.yScale);
            this.chart.svg.append('g')
                .attr('transform', `translate(${margin.left},${margin.top})`)
                .attr('class', 'y ala-chart-grid')
                .call(this.chart.yGrid);

            // y-axis drawing
            this.chart.yAxis = d3.axisLeft(this.chart.yScale);
            this.chart.svg.append('g')
                .attr('transform', `translate(${margin.left},${margin.top})`)
                .attr('class', 'y ala-chart-axis')
                .call(this.chart.yAxis);

            return true;
        }

        _render() {
            const {
                svg,
                margin,
                xScale,
                xAxis,
                yScale,
                yAxis,
                yGrid
            } = this.chart;
            const { width, height } = this.chart.dimensions();

            // update the scales from data
            xScale.domain(this.data().map(d => d.date.valueOf()));
            xAxis.tickFormat(d => new Date(d).getUTCDate());
            svg.select('.x.ala-chart-axis').transition().duration(300).call(xAxis);

            let yMax = this.eventCountMax();
            yMax += yMax % 4;
            yScale.domain([0, yMax]);

            yGrid.ticks(3)
                .tickSizeInner(width)
                .tickSizeOuter(0)
                .tickFormat('');
            svg.select('.y.ala-chart-grid').call(yGrid);

            yAxis.ticks(3);
            svg.select('.y.ala-chart-axis').call(yAxis);

            // bars
            const bars = svg.selectAll('.ala-chart-bar')
                .data(this.data(), d => d.date.valueOf());

            bars.enter()
                .append('rect')
                .attr('class', 'ala-chart-bar')
                .attr('x', d => margin.left + xScale(d.date.valueOf()))
                .attr('width', xScale.bandwidth)
                .attr('y', d => margin.top + yScale(d.count()))
                .attr('height', d => height - yScale(d.count()));

            bars.exit()
                .remove();

            // bars.transition()
            //     .duration(300)
            //     .attr('x', d => xScale(d.date.valueOf()))
            //     .attr('width', this.dataCount())
            //     .attr('y', d => yScale(d.count()))
            //     .attr('height', d => height - yScale(d.count()));

            // bar.append('text')
            //     .attr('x', d => xScale(d.date.valueOf()))
            //     .attr('y', height)
            //     .attr('dy', '.35em')
            //     .text(d => d.date.format('D'));
        }
    },
    template: `
        <div class="ala-daily-event-chart-area">
            <div data-bind="attr: { id: id }" class="ala-daily-event-chart"></div>
        </div>
    `
};
