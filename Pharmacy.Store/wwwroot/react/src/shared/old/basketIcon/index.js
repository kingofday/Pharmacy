import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import strings from './../constant';

class BasketIcon extends React.Component {
    constructor(props) {
        super(props);
        this.state = { animate: false }
    }
    componentDidUpdate(prevProps) {
        if (this.props.items.length !== prevProps.items.length) {
            this.setState(p => ({ ...p, animate: true }));
            let context = this;
            setTimeout(() => {
                context.setState(p => ({ ...p, animate: false }));
            }, 1000);
        }
    }
    render() {
        return (
            <div className='basket-icon-comp'>
                <Link to={this.props.route}>
                    <i className='icon zmdi zmdi-shopping-basket'></i>
                    <span> {strings.basket}</span>
                    <span className={'count' + (this.state.animate ? ' ripple-loader' : '')}> ({this.props.items.length})</span>
                </Link>
            </div>
        );
    }
}


const mapStateToProps = state => {
    return { ...state.basketReducer };
}

// const mapDispatchToProps = dispatch => ({
// });

export default connect(mapStateToProps, null)(BasketIcon);