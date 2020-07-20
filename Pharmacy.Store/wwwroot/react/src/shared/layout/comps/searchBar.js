import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import SearchDrug from '../../searchDrug/searchDrug';
import { LogOutAction } from './../../../redux/actions/authenticationAction';
import prescriptImage from './../../../assets/images/prescription.png';

class SearchBar extends React.Component {
    constructor(props) {
        super(props);
        this.state = { animate: false }
    }



    render() {
        return (
            <section id='comp-search-bar'>
                <Container>
                    <Row>
                        <Col sm={6} className='prescript-wrapper d-none d-sm-flex'>
                            <Link to='/prescript'>
                                <img src={prescriptImage} alt='pharmacy logo' />
                            </Link>
                        </Col>
                        <Col xs={12} sm={6} className='search-wrapper'>
                            <SearchDrug />
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