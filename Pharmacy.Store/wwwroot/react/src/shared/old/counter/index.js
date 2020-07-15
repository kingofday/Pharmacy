import React from 'react';

export default class Counter extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            count: this.props.count
        };
    }

    _plusCount() {
        this.props.onChange(this.props.id, this.state.count + 1);
        this.setState(p => ({ ...p, count: p.count + 1 }));

    }

    _minusCount() {
        if (this.state.count === 1) return;
        this.props.onChange(this.props.id, this.state.count - 1);
        this.setState(p => ({ ...p, count: p.count - 1 }));
    }
    render() {
        return (
            <div className={'counter-comp ' + this.props.className}>
                <button disabled={this.state.loading} className='btn-plus' onClick={this._plusCount.bind(this)}>+</button>
                <span className='count'>{this.state.count}</span>
                <button disabled={this.state.loading} className='btn-minus' onClick={this._minusCount.bind(this)}>-</button>
            </div>
        );
    }
}