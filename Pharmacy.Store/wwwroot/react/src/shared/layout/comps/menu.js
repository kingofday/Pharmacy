import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import strings from './../../constant';

class Menu extends React.Component {
    constructor(props) {
        super(props);
        this.state = { animate: false }
    }



    render() {
        return (
            <section id='comp-search-bar'>
                <Container>
                    <Row class="main-nav row">
                        <Col xs={12} sm={12} className='d-flex justify-content-between'>

                            <label class="menu-icon" for="toggle"><i class="zmdi zmdi-menu"></i></label>
                            <input type="checkbox" name="toggle" id="toggle" class="d-none" />
                            <ul class="animated">
                                <li class="close-menu"><label for="toggle"><i class="zmdi zmdi-close"></i></label></li>
                                <li class="navlink"><Link to='/'>{strings.home}</Link></li>
                                <li class="navlink"><Link to='/products'>{strings.products}</Link></li>
                                <li class="navlink"><Link to='/contactus'>{strings.contactus}</Link></li>
                                <li class="navlink"><Link to='/prescription'>{strings.sendPrescription}</Link></li>
                            </ul>
                            <div class="float-left categories" >

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

export default connect(mapStateToProps, mapDispatchToProps)(SearchBar);