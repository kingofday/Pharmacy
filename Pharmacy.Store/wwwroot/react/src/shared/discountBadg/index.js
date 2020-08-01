import React from 'react';
import { commaThousondSeperator } from './../../shared/utils';

export default class DiscountBadg extends React.Component {

    render() {
        if (this.props.discount == null || this.props.discount == 0) return null;
        return (
            <span className='discount-badg'>{commaThousondSeperator(this.props.discount)}<i className='zmdi zmdi-money-off'></i></span>
        );
    }
}