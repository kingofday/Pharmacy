import React from 'react';
import { connect } from 'react-redux'
import { TextField } from '@material-ui/core';
import strings from './../../../shared/constant';
import { Row, Col, Alert } from 'react-bootstrap';
import { LogInAction } from './../../../redux/actions/authAction';
import Button from './../../../shared/Button';

class Confirm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            message: {
                variant: '',
                text: ''
            },
            code: {
                value: '',
                error: false,
                errorMessage: ''
            }
        }
    }

    _inputChanged(e) {
        let state = this.state;
        state[e.target.id].value = e.target.value;
        state[e.target.id].error = false;
        state[e.target.id].errorMessage = '';
        this.setState((p) => ({ ...state }));
    }

    submit(e) {
    }

    render() {
        return (
            <div id="comp-login" className='card padding'>
                <Row className='ltr-elm'>
                    <Col xs={12} sm={12} md={{ span: 4, offset: 4 }} className='flex-column'>
                        <div className="form-group">
                            {
                                this.state.message.variant !== '' ?
                                    (<Alert variant={this.state.message.variant}>
                                        <p className="text-center">{this.state.message.text}</p>
                                    </Alert>) : null
                            }
                        </div>
                        <div className="form-group">
                            <TextField
                                error={this.state.code.error}
                                id="confirmCode"
                                label={strings.confirmCode}
                                value={this.state.code.value}
                                onChange={this._inputChanged.bind(this)}
                                helperText={this.state.code.errorMessage}
                                style={{ fontFamily: 'iransans' }}
                                variant="outlined"
                            />
                        </div>
                        <div className="btn-group">
                            <Button className='text-center w-100' onClick={this.submit.bind(this)}>{strings.confirm}</Button>
                        </div>
                    </Col>
                </Row>

            </div>
        );
    }
}

// const mapStateToProps = state => {
//     return { ...state.headerReducer, ...state.authenticationReducer };
// }

const mapDispatchToProps = dispatch => ({
    logIn: (model) => { dispatch(LogInAction(model)); }
});

export default connect(null, mapDispatchToProps)(Confirm);
