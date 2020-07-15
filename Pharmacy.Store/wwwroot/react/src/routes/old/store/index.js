import React, { Component } from 'react';
import { connect } from 'react-redux';
import ProductsList from './comps/productsList';
import Info from './comps/info';
import { Nav, Tab, Container, Row, Col } from 'react-bootstrap';
import strings from './../../shared/constant';
import { HideInitErrorAction } from './../../redux/actions/InitErrorAction';
import { SetBasketRouteAction } from './../../redux/actions/basketAction';
import { SetTempBasketIdAction, ClearTempBasketAction } from './../../redux/actions/tempBasketAction';

class Store extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            info: {},
            products: []
        };
    }

    componentDidMount() {
        localStorage.setItem('storeId', this.props.match.params.id);
        this.props.hideInitError();
        this.props.setBasketId(null);
        this.props.setBasketRoute('/basket');
        this.props.clearTempBasket()
    }

    render() {
        const { params } = this.props.match;
        return (
            <div className="store-page">
                <Info id={this.props.match.params.id} />
                <Tab.Container id="tabs" defaultActiveKey="all">
                    <Nav variant="tabs">
                        <Container>
                            <Row>
                                <Col>
                                    <Nav.Item>
                                        <Nav.Link eventKey="all">{strings.all}</Nav.Link>
                                    </Nav.Item>
                                    {/* <Nav.Item>
                                        <Nav.Link eventKey="mostVisited"><i className="icon most-visited-icon zmdi zmdi-flash"></i> {strings.mostVisited}</Nav.Link>
                                    </Nav.Item> */}
                                    <Nav.Item>
                                        <Nav.Link eventKey="favorites"><i className="icon favorites-icon zmdi zmdi-favorite"></i> {strings.favorites}</Nav.Link>
                                    </Nav.Item>
                                    <Nav.Item>
                                        <Nav.Link eventKey="best-sellers"><i className='icon best-sellers-icon zmdi zmdi-fire'></i> {strings.bestSellers}</Nav.Link>
                                    </Nav.Item>
                                </Col>
                            </Row>
                        </Container>
                    </Nav>
                    <Container>
                        <Row>
                            <Col>
                                <Tab.Content>
                                    <Tab.Pane eventKey="all" className="all">
                                        <ProductsList storeId={params.id} category='all' />
                                    </Tab.Pane>
                                    {/* <Tab.Pane eventKey="mostVisited">
                                        <ProductsList storeId={params.id} category='mostVisited' />
                                    </Tab.Pane> */}
                                    <Tab.Pane eventKey="favorites">
                                        <ProductsList storeId={params.id} category='favorites' />
                                    </Tab.Pane>
                                    <Tab.Pane eventKey="best-sellers">
                                        <ProductsList storeId={params.id} category='bestSellers' />
                                    </Tab.Pane>
                                </Tab.Content>
                            </Col>
                        </Row>
                    </Container>

                </Tab.Container>
            </div>

        );
    };

}

// const mapStateToProps = state => {
//     return { ...state.initErrorReducer };
// }

const mapDispatchToProps = dispatch => ({
    hideInitError: () => dispatch(HideInitErrorAction()),
    setBasketRoute: (route) => dispatch(SetBasketRouteAction(route)),
    setBasketId: (id) => dispatch(SetTempBasketIdAction(id)),
    clearTempBasket: () => dispatch(ClearTempBasketAction())
});

export default connect(null, mapDispatchToProps)(Store);