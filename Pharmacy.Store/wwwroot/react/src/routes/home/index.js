import React from 'react';
import { Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';
import SearchDrug from './../../shared/searchDrug/searchDrug';
import { ShowInitErrorAction } from '../../redux/actions/InitErrorAction';
import strings, { enums } from '../../shared/constant';

import DrugSlideShow from './comps/drugSlideShow';

import ThirdRow from './comps/thirdRow';
import DrugStores from './comps/drugStores';
import Categories from './../../shared/categories/categories';

class Home extends React.Component {

    render() {
        return (
            <div id='home-page'>
                <Container>
                    <Row id='first-row' className='mb-15'>
                        <Col xs={12} lg={9}>
                            <div id='search-box'>
                                <h1 className='hx1'>خرید دارو بصورت آنلاین</h1>
                                <h2 className='hx2'>مشـاوره رایـگان توسط بهتریـن دکتر های داروسـاز</h2>
                                <div className='search-wrapper'>
                                    <SearchDrug />
                                </div>
                            </div>
                        </Col>
                        <Col className='d-none d-lg-flex' lg={3}>
                            <div className='r-bg'></div>
                        </Col>
                    </Row>
                    <Row id='second-row' className='mb-15'>
                        <Col xs={12} lg={9}>
                            <div className='card mb-15 w-100' style={{ direction: 'ltr' }}>
                                <DrugSlideShow speed={700} title={strings.bestSellers} type={enums.drugFilterType.bestSellers.value} />
                            </div>
                        </Col>
                        <Col className='categories d-none d-lg-flex' lg={3}>
                            <Categories />
                        </Col>
                    </Row>
                    <ThirdRow />
                    <DrugStores />
                    <Row>
                        <Col xs={12} sm={12}>
                            <div className='card mb-15 w-100'>
                                <DrugSlideShow title={strings.newests} type={enums.drugFilterType.newest.value} />
                            </div>
                        </Col>
                    </Row>
                </Container>
            </div>
        );
    }
}


const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message))
});

export default connect(null, mapDispatchToProps)(Home);