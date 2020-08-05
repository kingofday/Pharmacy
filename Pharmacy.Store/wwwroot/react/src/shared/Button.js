import React from 'react';

export default class extends React.Component {
    render() {
    return (<button disabled={this.props.disabled} className={this.props.className} onClick={this.props.onClick}>{this.props.children}{this.props.disabled?'...':''}</button>);
    }
}