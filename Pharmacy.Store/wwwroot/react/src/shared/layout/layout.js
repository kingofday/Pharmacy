import React from 'react';
import { connect } from 'react-redux'
import { ToastContainer } from 'react-toastify';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';

import TopHeader from './comps/topHeader';
import SearchBar from './comps/searchBar';
import Menu from './comps/menu';
import LinksWithInfo from './comps/linksWithInfo';
import UsefulLinks from './comps/usefulLinks';
import Footer from './comps/footer';

import AuthRouter from './comps/authRoute';
import Products from '../../routes/products/products';
import Product from '../../routes/product';
import Basket from './../../routes/basket';
import TempBasket from '../../routes/tempBasket';
import DeliveryPayment from '../../routes/deliveryPayment';
import Auth from './../../routes/auth/auth';
import NotFound from '../../routes/notFound';
import InitError from '../initError';
import Home from '../../routes/home';
import SelectAddress from '../../routes/selectAddress';
import SelectLocation from '../../routes/selectLocation';
import SelectDelivery from '../../routes/selectDelivery';
import Review from './../../routes/review';
import Prescription from './../../routes/prescription';
import AfterGateway from '../../routes/afterGateway';


class Layout extends React.Component {
    render() {
        return (
            <Router className="layout">
                <TopHeader />
                <SearchBar />
                <Menu />
                <Switch>
                    <Route exact path="/" component={Home} />
                    <Route exact path="/products" component={Products} />
                    <Route exact path="/product/:id" component={Product} />
                    <Route exact path="/basket" component={Basket} />
                    <Route exact path="/auth" component={Auth} />
                    <AuthRouter path="/selectAddress" component={SelectAddress}/>
                    <AuthRouter path="/selectLocation" component={SelectLocation}/>
                    <AuthRouter path="/selectDelivery" component={SelectDelivery}/>
                    <AuthRouter path="/review" component={Review}/>
                    <Route exact path="/prescription" component={Prescription} />
                    <Route exact path="/afterGateway/:status/:orderId/:transId" component={AfterGateway} />
                    <Route exact path="/tempBasket/:id?" component={TempBasket} />
                    <Route exact path="/deliveryPayment/:id?" component={DeliveryPayment} />
                    {/* <Route exact path="/contactus" component={ContactUs} /> */}
                    {/* <Route exact path="/contactus" component={ContactUs} />
                   
                    <Route exact path="/tempbasket/:basketId?" component={TempBasket} />
                    <Route path="/completeInformation" component={CompleteInfo} />
                   
                    <Route path="/review" component={Review} />*/}
                    <Route path="/:msg?" component={NotFound} />
                </Switch>
                <LinksWithInfo />
                <UsefulLinks />
                <Footer />
                <InitError />
                <ToastContainer containerId={'common_toast'} rtl />
            </Router>
        );
    }
}

export default connect(null, null)(Layout);