import React from 'react';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'react-bootstrap';
import { connect } from 'react-redux';
import SearchDrug from './../../shared/searchDrug/searchDrug';
import Skeleton from '@material-ui/lab/Skeleton';
import { ShowInitErrorAction } from '../../redux/actions/InitErrorAction';
import strings from '../../shared/constant';
import srvCategory from './../../service/srvCategory';

class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            categories: []
        }
    }
    async _fetchCategories() {
        let srvRep = await srvCategory.get();
        if (srvRep.success) {
            this.setState(p => ({ ...p, categories: srvRep.result }));
        }
    }

    async _fetchData() {
        // let srvRep = await generalSrv.getContactUsInfo();
        // if (!srvRep.success) {
        //     this.props.showInitError(this._fetchData.bind(this), srvRep.message);
        //     return;
        // }
        //this.setState(p => ({ ...p, ...srvRep.result, loading: false }));
    }

    async componentDidMount() {
        console.log('here');
        await this._fetchData();
        await this._fetchCategories();
    }

    render() {
        return (
            <div id='home-page'>
                <Container>
                    <Row id='first-row' className='mb-15'>
                        <Col xs={12} lg={8}>
                            <div id='search-box'>
                                <h1 className='hx1'>خرید دارو بصورت آنلاین</h1>
                                <h2 className='hx2'>مشـاوره رایـگان توسط بهتریـن دکتر های داروسـاز</h2>
                                <div className='search-wrapper'>
                                    <SearchDrug />
                                </div>
                            </div>
                        </Col>
                        <Col className='d-none d-lg-flex' lg={4}>
                            <div className='r-bg'></div>
                        </Col>
                    </Row>
                    <Row id='second-row' className='mb-15'>
                        <Col xs={12} lg={8}>

                        </Col>
                        <Col className='categories d-none d-lg-flex' lg={4}>
                            <div className='card mb-15'>
                                <h3 className='heading'>
                                    دسته بندی ها
                                </h3>
                                <ul>
                                    {this.state.categories.length === 0 ? ([0, 1, 2, 3].map(idx => (<li key={idx}><Skeleton height={30} variant='rect' /></li>))) :
                                        this.state.categories.map((item, idx) => (<li key={idx}>
                                            <Link to={`/products/${item.categoryId}`}>
                                                {item.name}
                                            </Link>
                                        </li>))
                                    }
                                </ul>
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