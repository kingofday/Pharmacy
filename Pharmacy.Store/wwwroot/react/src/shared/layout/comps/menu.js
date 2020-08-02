import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import strings from './../../constant';
import { LogOutAction } from './../../../redux/actions/authAction';
import Categories from './categories';

class Menu extends React.Component {
    constructor(props) {
        super(props);
        this.state = { animate: false }
    }

    render() {
        return (
            <section id='comp-menu'>
                <Container>
                    <Row className="main-nav">
                        <Col xs={12} sm={12} className='d-flex justify-content-between'>

                            <label className="menu-icon" htmlFor="toggle"><i className="zmdi zmdi-menu"></i></label>
                            <input type="checkbox" name="toggle" id="toggle" className="d-none" />
                            <ul className="menu-items animated">
                                <li className="close-menu"><label htmlFor="toggle"><i className="zmdi zmdi-close"></i></label></li>
                                <li className="navlink"><Link to='/'>{strings.home}</Link></li>
                                <li className="navlink"><Link to='/products'>{strings.products}</Link></li>
                                <li className="navlink"><Link to='/contactus'>{strings.contactus}</Link></li>
                                <li className="navlink"><Link to='/prescription'>{strings.sendPrescription}</Link></li>
                            </ul>
                            <div className="float-left categories-wrapper" >
                                <Categories />
                            </div>


                        </Col>

                    </Row>
                </Container>
            </section>

        );
    }
}

const mapStateToProps = state => {
    return { ...state.basketReducer, ...state.authenticationReducer };
}

const mapDispatchToProps = dispatch => ({
    logOut: () => dispatch(LogOutAction())
});

export default connect(mapStateToProps, mapDispatchToProps)(Menu);