import React from 'react';


export default class DiscountBadg extends React.Component {

    render() {
        if (this.props.discount == null || this.props.discount == 0) return null;
        return (
            <span className='discount-badg'>{this.props.discount}%</span>
        );
    }
}