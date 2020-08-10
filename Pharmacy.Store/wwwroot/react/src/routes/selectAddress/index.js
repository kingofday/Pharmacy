import React from 'react';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { TextField } from '@material-ui/core';
import { Container, Row, Col } from 'react-bootstrap';
import Skeleton from '@material-ui/lab/Skeleton';
import CustomMap from '../../shared/map';
import { Radio, FormControlLabel, RadioGroup } from '@material-ui/core';

//import Steps from './../../shared/steps';
import strings, { validationStrings } from './../../shared/constant';
import AddressListModal from './comps/addressListModal';
import { Redirect, Link } from 'react-router-dom';
import { SetAddrssAction } from './../../redux/actions/addressAction';
import { ShowInitErrorAction, HideInitErrorAction } from './../../redux/actions/InitErrorAction';
import { commaThousondSeperator } from './../../shared/utils';
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
            deliveryId: '',
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
        this.setState(p => ({ ...p, prevAddress: item, lng: null, lat: null, deliveryId: '', deliveryCost: null, placeName: null }));
    }

    _remmoveAddress() {
        this.setState(p => ({ ...p, prevAddress: null }));
    }

    _fetchData() {

    }
    async componentDidMount() {
        this.props.hideInitError();
    }

    async _showModal() {
        await this.modal._toggle();
    }

    _selectDeliveryType(e) {
        let deliveryId = e.target.value;
        let type = this.state.deliveryTypes.find(x => x.id === parseInt(deliveryId));
        this.setState(p => ({ ...p, deliveryId: deliveryId, deliveryCost: type.cost }));
    }

    async _submit() {

        if (!this.state.prevAddress) {
            if (!this.state.location.lng || !this.state.location.lat) {
                this.setState(p => ({ ...p, location: { ...p.location, message: validationStrings.required } }));
                return;
            }
            if (!this.state.address.value) {
                this.setState(p => ({ ...p, address: { ...p.address, error: true, message: validationStrings.required } }))
                return;
            }
        }
        if (!this.state.prevAddress) {
            this.props.setAddress({
                address: this.state.address.value,
                lng: this.props.lng,
                lat: this.props.lat
            },
                this.state.reciever.value,
                this.state.recieverMobileNumber.value,
                this.state.deliveryId,
                this.state.deliveryCost
            );
        }
        else {
            this.props.setAddress(this.state.prevAddress,
                this.state.reciever.value,
                this.state.recieverMobileNumber.value,
                this.state.deliveryId,
                this.state.deliveryCost
            );
        }
        addressSrv.saveInfo(this.state.reciever.value, this.state.recieverMobileNumber.value);
        this.setState(p => ({ ...p, redirect: '/review' }));

    }

    render() {
        if (this.state.redirect) return <Redirect to={this.state.redirect} />;
        return (
            <div id='page-select-address' className="page-comp">

                <Container>
                    <Row>
                        <Col xs={12}>
                            <div className='card padding w-100'>
                                {this.state.prevAddress ? (
                                    <Row className='m-b'>
                                        <Col xs={10}>
                                            <RadioGroup aria-label="address" name="old-address" value={this.state.prevAddress.id.toString()}>
                                                <FormControlLabel value={this.state.prevAddress.id.toString()} control={<Radio color="primary" />} label={this.state.prevAddress.address} />
                                            </RadioGroup>
                                        </Col>
                                        <Col xs={2} className='d-flex align-items-center'>
                                            <button className='btn-remove-address' onClick={this._remmoveAddress.bind(this)}>
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
                                                    <i className='zmdi zmdi-google-maps'></i>
                                                </label>
                                            </Link>
                                            <p className='Mui-error'>{this.state.location.message}</p>
                                        </Col>
                                        <Col xs={12}>
                                            <div className="form-group">
                                                <TextField
                                                    id="address"
                                                    error={this.state.details.error}
                                                    label={strings.address}
                                                    multiline
                                                    rows={2}
                                                    value={this.state.details.value}
                                                    onChange={this._inputChanged.bind(this)}
                                                    helperText={this.state.details.message}
                                                    variant="outlined" />
                                            </div>
                                        </Col>
                                    </Row>)}

                                <Row>
                                    <Col className='d-flex justify-content-end m-b'>
                                        <button onClick={this._showModal.bind(this)}>{strings.previouseAddresses}</button>
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
                                                label={strings.mobileNumber}
                                                value={this.state.mobileNumber.value}
                                                onChange={this._inputChanged.bind(this)}
                                                helperText={this.state.mobileNumber.message}
                                                style={{ fontFamily: 'iransans' }}
                                                variant="outlined" />
                                        </div>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        {this.state.loading ? [0, 1].map((x) => <Skeleton className='m-b' key={x} variant='rect' height={25} />) :
                                            <RadioGroup aria-label="address" name="old-address" value={this.state.deliveryId} onChange={this._selectDeliveryType.bind(this)}>
                                                {this.state.deliveryTypes.map((d) => <FormControlLabel key={d.id} value={d.id.toString()} control={<Radio color="primary" />} label={`${d.name} (${d.cost} ${strings.currency})`} />)}
                                            </RadioGroup>}
                                    </Col>
                                </Row>
                            </div>
                        </Col>
                    </Row>

                </Container>
                <button className='btn-next' onClick={this._submit.bind(this)} disabled={this.state.loading}>
                    {strings.continuePurchase}
                </button>
                <AddressListModal ref={(comp) => this.modal = comp} onChange={this._selectAddress.bind(this)} />
            </div >
        );
    }

}
const mapStateToProps = state => {
    return { ...state.mapReducer, ...state.basketReducer };
}

const mapDispatchToProps = dispatch => ({
    hideInitError: () => dispatch(HideInitErrorAction()),
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message)),
    setAddress: (address, reciever, recieverMobileNumber, deliveryId, deliveryCost) => dispatch(SetAddrssAction(address, reciever, recieverMobileNumber, deliveryId, deliveryCost))
});

export default connect(mapStateToProps, mapDispatchToProps)(SelectAddress);
