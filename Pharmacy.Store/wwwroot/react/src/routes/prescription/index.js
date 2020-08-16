import React from 'react';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { TextField } from '@material-ui/core';
import { Container, Row, Col } from 'react-bootstrap';
import Skeleton from '@material-ui/lab/Skeleton';
import CustomMap from '../../shared/map';
import { Radio, FormControlLabel, RadioGroup } from '@material-ui/core';

import Button from './../../shared/Button';
import strings, { validationStrings } from './../../shared/constant';
import AddressListModal from './comps/addressListModal';
import { Redirect, Link } from 'react-router-dom';
import { SetAddrssAction } from './../../redux/actions/reviewAction';
import { ShowInitErrorAction, HideInitErrorAction } from './../../redux/actions/InitErrorAction';
import srvPrescription from './../../service/srvPrescription';


class Prescription extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            redirect: false,
            images: [],
            mobileNumber: {
                value: '',
                error: false,
                errorMessage: ''
            }

        };
    }

    _inputChanged(e) {
        let state = this.state;
        if (e.target.name) state[e.target.name].value = e.target.value;
        else state[e.target.id].value = e.target.value;
        this.setState((p) => ({ ...p, ...state }));
    }


    async componentDidMount() {
        this.props.hideInitError();
    }

    async _submit() {
        let addr = null;
        this.setState(p => ({ ...p, loading: true }));

        this.setState(p => ({ ...p, loading: false, redirect: '/selectDelivery' }));

    }
    _select() {
        this.
    }
    _uploaderChanged() {

    }

    render() {
        if (this.state.redirect) return <Redirect to={this.state.redirect} />;
        return (
            <div id='page-prescription' className="page-comp">
                <Container>
                    <Row>
                        <Col xs={12}>
                            <div className='card padding w-100'>
                                <Row>
                                    <Col xs={12}>
                                        <input type='file' multiple
                                            className='d-none' id='file'
                                            ref={c => this.uploader = c}
                                            onChange={this._uploaderChanged}
                                            accept="image/*" />
                                        <div className="form-group">
                                            <button id='uplaoder' onClick={this._select.bind(this)}>
                                                <i className='zmdi zmdi-cloud-upload'></i>
                                                <span>{strings.prescription}</span>
                                            </button>
                                        </div>
                                    </Col>
                                    <Col xs={12}>
                                        {this.state.images.map((image, idx) => (<div key={idx} className="uploaded-box">
                                            <img src={image} />
                                            <button onClick={() => this.removeFile(idx)}><i className="zmdi zmdi-close"></i></button>
                                        </div>))}
                                    </Col>
                                    <Col xs={12}>
                                        <div className="form-group">
                                            <TextField
                                                error={this.state.mobileNumber.error}
                                                id="username"
                                                label={strings.mobileNumber}
                                                placeholder='9xxxxxxxxx'
                                                value={this.state.mobileNumber.value}
                                                onChange={this._inputChanged.bind(this)}
                                                helperText={this.state.mobileNumber.errorMessage}
                                                style={{ fontFamily: 'iransans' }}
                                                variant="outlined"
                                            />
                                        </div>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col xs={12} sm={12} className='d-flex justify-content-end'>
                                        <Button onClick={this._submit.bind(this)} loading={this.state.loading}>
                                            {strings.continuePurchase}
                                        </Button>
                                    </Col>
                                </Row>
                            </div>
                        </Col>
                    </Row>

                </Container>
                <AddressListModal ref={(comp) => this.modal = comp} onChange={this._selectAddress.bind(this)} />
            </div >
        );
    }

}
const mapStateToProps = state => {
    return { ...state.authReducer, ...state.mapReducer, ...state.basketReducer };
}

const mapDispatchToProps = dispatch => ({
    hideInitError: () => dispatch(HideInitErrorAction()),
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message)),
    setAddress: (addr) => dispatch(SetAddrssAction(addr))
});

export default connect(mapStateToProps, mapDispatchToProps)(SelectAddress);
