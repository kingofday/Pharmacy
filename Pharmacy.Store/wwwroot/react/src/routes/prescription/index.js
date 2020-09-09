import React from 'react';
import { connect } from 'react-redux';
import { toast } from 'react-toastify';
import { TextField } from '@material-ui/core';
import { Container, Row, Col, Alert } from 'react-bootstrap';
import { validate } from './../../shared/utils';
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
            files: [],
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
        if (this.state.files.length === 0) {
            toast(validationStrings.atleastOneFileRequired, { type: toast.TYPE.ERROR });
            return;
        }
        if (!this.props.authenticated) {
            if (!this.state.mobileNumber.value) {
                this.setState(p => ({ ...p, mobileNumber: { ...p.mobileNumber, error: true, errorMessage: validationStrings.required } }));
                return;
            }
            if (!validate.mobileNumber(this.state.mobileNumber.value)) {
                this.setState(p => ({ ...p, mobileNumber: { ...p.mobileNumber, error: true, errorMessage: validationStrings.invalidMobileNumber } }));
                return;
            }
        }
        this.setState(p => ({ ...p, loading: true }));
        let add = await srvPrescription.add(this.state.mobileNumber.value, this.state.files);
        console.log(add);
        this.setState(p => ({ ...p, loading: false }));
        if (!add.success) {
            toast(add.message, { type: toast.TYPE.ERROR });
            return;
        }
        toast(strings.submitPrescriptionSuccessfully, { type: toast.TYPE.SUCCESS });
        this.setState(p => ({ ...p, images: [], files: [], mobileNumber: { value: '', error: false } }))

    }
    _select() {
        this.uploader.click();
    }
    _uploaderChanged(event) {
        let files = event.target.files;

        if (files) {
            if (this.state.files.length + files.length > 5) {
                toast(validationStrings.maxFileCountExceeded(5), { type: toast.TYPE.ERROR });
                return;
            }
            for (var i = 0; i < files.length; i++) {
                let chk = this._checkFile(files[i]);
                if (!chk.success) {
                    toast(chk.message, { type: toast.TYPE.ERROR });
                    break;
                }
                let reader = new FileReader();
                reader.onload = function (e) {
                    this.setState(p => ({ images: [...p.images, e.target.result] }));
                }.bind(this);
                reader.readAsDataURL(files[i]);
            }
            this.setState(p => ({ ...p, files: [...p.files, ...files] }));

        }

    }

    _checkFile(file) {
        console.log(file);
        if (file.size > 1024 * 1024 * 5) return { success: false, message: validationStrings.maxFileSizeExceeded(5) };
        else return { success: true };

    }

    _removeFile(idx) {
        let images = this.state.images;
        images.splice(idx, 1);
        let files = this.state.files;
        files.splice(idx, 1);
        this.setState(p => ({ ...p, images: [...images], files: [...files] }));
    }

    render() {
        if (this.state.redirect) return <Redirect to={this.state.redirect} />;
        return (
            <div id='page-prescription' className="page-comp">
                <Container>
                    <Row>
                        <Col xs={12}>
                            <div className='card w-100'>
                                <Row className="ltr-elm">
                                    <Col className='d-flex flex-column' xs={12} sm={12} md={{ span: 6, offset: 3 }} lg={{ span: 4, offset: 4 }}>
                                        <Row>
                                            <Col xs={12}>
                                                <Alert className='text-center' variant='info'>{strings.prescriptionGuid}</Alert>
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col xs={12} className="mb-15">
                                                <input type='file' multiple
                                                    className='d-none' id='file'
                                                    ref={c => { this.uploader = c; }}
                                                    onChange={this._uploaderChanged.bind(this)}
                                                    accept="image/*" />
                                                <button id='uploader' onClick={this._select.bind(this)}>
                                                    <i className='zmdi zmdi-cloud-upload'></i>
                                                    <span>{strings.prescriptionImage}</span>
                                                </button>
                                            </Col>
                                            <Col xs={12} className="d-flex mb-15">
                                                {this.state.images.map((image, idx) => (<div key={idx} className="uploaded-box">
                                                    <img src={image} />
                                                    <button onClick={() => this._removeFile(idx)}><i className="zmdi zmdi-close"></i></button>
                                                </div>))}
                                            </Col>
                                            {this.props.authenticated ? null : <Col xs={12}>
                                                <div className="form-group">
                                                    <TextField
                                                        error={this.state.mobileNumber.error}
                                                        id="mobileNumber"
                                                        label={strings.mobileNumber}
                                                        placeholder='9xxxxxxxxx'
                                                        value={this.state.mobileNumber.value}
                                                        onChange={this._inputChanged.bind(this)}
                                                        helperText={this.state.mobileNumber.errorMessage}
                                                        style={{ fontFamily: 'iransans' }}
                                                        variant="outlined"
                                                    />
                                                </div>
                                            </Col>}

                                        </Row>
                                        <Row>
                                            <Col xs={12} sm={12} className='d-flex justify-content-center'>
                                                <Button className='btn-next' onClick={this._submit.bind(this)} loading={this.state.loading}>
                                                    {strings.send}
                                                </Button>
                                            </Col>
                                        </Row>
                                    </Col>
                                </Row>

                            </div>
                        </Col>
                    </Row>

                </Container>
            </div >
        );
    }

}
const mapStateToProps = (state, ownProps) => {
    return { ...ownProps, ...state.authReducer };
}

const mapDispatchToProps = dispatch => ({
    hideInitError: () => dispatch(HideInitErrorAction())
});

export default connect(mapStateToProps, mapDispatchToProps)(Prescription);
