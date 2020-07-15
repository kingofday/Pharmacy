import React from 'react';
import strings from './../../shared/constant';
import { connect } from 'react-redux';
import { HideInitErrorAction } from './../../redux/actions/InitErrorAction';

class InitError extends React.Component {

    _fetchData() {
        if (this.props.fetchDatas.length > 0) {
            this.props.hide();
            this.props.fetchDatas.forEach(async function (fd) {
                await fd();
            })
        }

    }

    render() {
        return (
            <div className='retry-comp' style={{ display: this.props.show ? 'flex' : 'none' }}>
                {/* <button onClick={} className='close'><i className='zmdi zmdi-close'></i></button> */}
                <label>{this.props.message ? this.props.message : strings.connecttionFailed}</label>
                <button onClick={this._fetchData.bind(this)} className='btn-retry'>
                    <i className='zmdi zmdi-refresh'></i>
                </button>
            </div>
        );
    }
}


const mapStateToProps = state => {
    return { ...state.initErrorReducer };
}

const mapDispatchToProps = dispatch => ({
    hide: () => dispatch(HideInitErrorAction())
});

export default connect(mapStateToProps, mapDispatchToProps)(InitError);