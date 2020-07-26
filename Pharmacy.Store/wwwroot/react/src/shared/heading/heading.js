import React from 'react';

export default class Heading extends React.Component {
    render() {
        return (
            <h3 className={'comp-heading ' + (this.props.className ? this.props.className : '')}>
                {this.props.title}
            </h3>
        );
    }
}