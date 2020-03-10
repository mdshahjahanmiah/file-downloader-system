import React, { Component } from 'react';
//import appsettings from '../../src/appsettings.json';
export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { downloads: [], loading: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderDownloadsTable(downloads) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Source</th>
                        <th>Destination</th>
                        <th>Download Started Date</th>
                        <th>Download Ended Date</th>
                        <th>Protocol</th>
                        <th>Large Data</th>
                        <th>Slow</th>
                        <th>Percentage Of Failure</th>
                        <th>Elapsed Time(S)</th>
                        <th>Download Speed(Mbps)</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {downloads.map(download =>
                        <tr key={download.fileId}>
                            <td>{download.source}</td>
                            <td>{download.destination}</td>
                            <td>{download.downloadStartedDate}</td>
                            <td>{download.downloadEndedDate}</td>
                            <td>{download.protocol}</td>
                            <td>{download.isLargeData}</td>
                            <td>{download.isSlow}</td>
                            <td>{download.percentageOfFailure}</td>
                            <td>{download.elapsedTime}</td>
                            <td>{download.downloadSpeed}</td>
                            <td>{download.status}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading ? <p><em>Loading...</em></p> : FetchData.renderDownloadsTable(this.state.downloads);
        return (
            <div>
                <h1 id="tabelLabel" >Download List</h1>
                {contents}
            </div>
        );
    }
    async populateWeatherData() {
        const response = await fetch('/FileDownloader');
        console.log(response);
        const data = await response.json();
        this.setState({ downloads: data, loading: false });
    }
}
