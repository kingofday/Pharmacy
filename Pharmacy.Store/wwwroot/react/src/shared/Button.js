import React from 'react';
import { Spinner} from 'react-bootstrap';

export default class extends React.Component {
    render() {
        return (<button disabled={this.props.disabled || this.props.loading} className={this.props.className} onClick={this.props.onClick}>{this.props.children}&nbsp;{this.props.loading ? <Spinner className='va-middle' animation="border" size="sm" /> : ''}</button>);
    }
}