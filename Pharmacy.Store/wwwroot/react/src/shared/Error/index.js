import React from 'react';

export default class Error extends React.Component {
    render() {
        return (
            <div className='error-comp'>
                <i className='zmdi zmdi-cloud-off'></i>
                <span className='text'>{this.props.message}</span>
            </div>
        );
    }
}
