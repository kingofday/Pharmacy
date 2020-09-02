import React from 'react';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { TextField } from '@material-ui/core';
import { Container, Row, Col } from 'react-bootstrap';
import Skeleton from '@material-ui/lab/Skeleton';
import CustomMap from '../../shared/map';
import { Radio, FormControlLabel, RadioGroup } from '@material-ui/core';

import Steps from './../../shared/steps';
import Button from './../../shared/Button';
import strings, { validationStrings } from './../../shared/constant';
import AddressListModal from './comps/addressListModal';
import { Redirect, Link } from 'react-router-dom';
import { SetAddrssAction } from './../../redux/actions/reviewAction';
import { ShowInitErrorAction, HideInitErrorAction } from './../../redux/actions/InitErrorAction';
import { commaThousondSeperator, validate } from './../../shared/utils';
import addressSrv from './../../service/srvAddress';

const inputs = ['mobileNumber', 'fullname', 'details'];

class SelectAddress extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            redirect: false,
            location: {
                lng: this.props.lng,
                lat: this.props.lat,
                message: null
            },
            placeName: '',
            prevAddress: '',
            deliveryTypes: []
        };
        for (let i = 0; i < inputs.length; i++)
            this.state[inputs[i]] = {
                value: '',
                error: false,
                errorMessage: ''
            };
    }

    _inputChanged(e) {
        let state = this.state;
        if (e.target.name) state[e.target.name].value = e.target.value;
        else state[e.target.id].value = e.target.value;
        this.setState((p) => ({ ...p, ...state }));
    }

    _mapChanged(lng, lat) {
        this.setState(p => ({ ...p, location: { lng: lng, lat: lat } }));
    }

    async _selectAddress(item) {
        this.setState(p => ({
            ...p,
            loading: false,
            prevAddress: item,
            location: { ...p.location, message: '' },
            fullname: { ...p.fullname, value: item.fullname||'' },
            mobileNumber: { ...p.mobileNumber, value: item.mobileNumber||'' },
            placeName: null
        }));
    }

    _remmoveAddress() {
        this.setState(p => ({ ...p, prevAddress: null }));
    }

    async componentDidMount() {
        this.props.hideInitError();
    }

    async _showModal() {
        await this.modal._toggle();
    }

    _validate() {
        let isValid = true;
        if (!this.state.prevAddress) {
            if (!this.state.location.lng || !this.state.location.lat) {
                this.setState(p => ({ ...p, location: { ...p.location, message: validationStrings.required } }));
                isValid = false;
            }
            if (!this.state.details.value) {
                this.setState(p => ({ ...p, details: { ...p.details, error: true, message: validationStrings.required } }))
                isValid = false;
            }
        }
        return isValid;
    }

    async _submit() {
        if (!this._validate())
            return
        let addr = null;
        this.setState(p => ({ ...p, loading: true }));
        let pAddr = this.state.prevAddress;
        if (!pAddr) {
            addr = {
                fullname: this.state.fullname.value,
                mobileNumber: this.state.mobileNumber.value,
                details: this.state.details.value,
                lat: this.state.location.lat,
                lng: this.state.location.lng
            };
            let add = await addressSrv.add(addr);
            if (!add.success) {
                toast(add.message, { type: toast.TYPE.ERROR });
                this.setState(p => ({ ...p, loading: false }));
                return;
            }
            addr.id = add.result;
        }
        else {
            addr = {
                ...this.state.prevAddress,
                fullname: this.state.fullname.value,
                mobileNumber: this.state.mobileNumber.value,
            };
            if (pAddr.fullname !== this.state.fullname.value || pAddr.mobileNumber !== this.state.mobileNumber.value) {
                let update = await addressSrv.update(addr);
                if (!update.success) {
                    toast(update.message, { type: toast.TYPE.ERROR });
                    this.setState(p => ({ ...p, loading: false }));
                    return;
                }
            }
        }

        this.props.setAddress(addr);
        this.setState(p => ({ ...p, loading: false, redirect: '/selectDelivery' }));

    }

    render() {
        if (this.state.redirect) return <Redirect to={this.state.redirect} />;
        return (
            <div id='page-select-address' className="page-comp">
                <Container>
                    <Row>
                        <Col xs={12}>
                            <div className='card padding w-100'>
                                <Row>
                                    <Col>
                                        <Steps />
                                    </Col>
                                </Row>
                                {this.state.prevAddress ? (
                                    <Row className='m-b'>
                                        <Col xs={10}>
                                            <RadioGroup aria-label="address" name="old-address" value={this.state.prevAddress.id.toString()}>
                                                <FormControlLabel value={this.state.prevAddress.id.toString()} control={<Radio color="primary" />} label={this.state.prevAddress.details} />
                                            </RadioGroup>
                                        </Col>
                                        <Col xs={2} className='d-flex align-items-center'>
                                            <button className='btn-remove-address btn-link' onClick={this._remmoveAddress.bind(this)}>
                                                <i className='zmdi zmdi-close'></i>
                                            </button>
                                        </Col>
                                    </Row>

                                ) :
                                    (<Row>
                                        <Col xs={12} className='m-b'>
                                            <Link className={'location-selector ' + (this.state.location.message ? 'error' : '')} to={`/selectLocation?lng=${this.props.lng}&lat=${this.props.lat}`}>
                                                <CustomMap height='50px' lng={this.props.lng} lat={this.props.lat} hideMarker={true} />
                                                <label>
                                                    <span>{this.state.placeName ? this.state.placeName : strings.selectLocation}</span>
                                                    {this.state.location.lng ? <i className='zmdi zmdi-check color-green icon'></i> : <i className='zmdi zmdi-google-maps'></i>}
                                                </label>
                                            </Link>
                                            <p className='Mui-error'>{this.state.location.message}</p>
                                        </Col>
                                        <Col xs={12}>
                                            <div className="form-group mb-0">
                                                <TextField
                                                    id="details"
                                                    error={this.state.details.error}
                                                    label={strings.addressDetails}
                                                    multiline
                                                    rows={1}
                                                    value={this.state.details.value}
                                                    onChange={this._inputChanged.bind(this)}
                                                    helperText={this.state.details.message}
                                                    variant="outlined" />
                                            </div>
                                        </Col>
                                    </Row>)}

                                <Row>
                                    <Col className='d-flex justify-content-end m-b'>
                                        <button className='btn-link' onClick={this._showModal.bind(this)}>{strings.previouseAddresses}</button>
                                    </Col>

                                </Row>
                                <Row>
                                    <Col xs={12} sm={6}>
                                        <div className="form-group">
                                            <TextField
                                                error={this.state.fullname.error}
                                                id="fullname"
                                                label={strings.recieverFullname}
                                                value={this.state.fullname.value}
                                                onChange={this._inputChanged.bind(this)}
                                                helperText={this.state.fullname.message}
                                                style={{ fontFamily: 'iransans' }}
                                                variant="outlined" />
                                        </div>
                                    </Col>
                                    <Col xs={12} sm={6}>
                                        <div className="form-group">
                                            <TextField
                                                error={this.state.mobileNumber.error}
                                                id="mobileNumber"
                                                name="mobileNumber"
                                                type='number'
                                                className='ltr-input'
                                                label={strings.recieverMobileNumber}
                                                value={this.state.mobileNumber.value}
                                                onChange={this._inputChanged.bind(this)}
                                                helperText={this.state.mobileNumber.message}
                                                style={{ fontFamily: 'iransans' }}
                                                variant="outlined" />
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
