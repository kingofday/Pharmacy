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

import Products from '../../routes/products/products';
import Product from '../../routes/product';
import Basket from './../../routes/basket';
import Auth from './../../routes/auth/auth';
// import Store from '../../routes/store';
// import ContactUs from '../../routes/contactUs';
// import TempBasket from './../../routes/tempBasket';
// import AfterGateway from '../../routes/afterGateway';
import NotFound from '../../routes/notFound';
import InitError from '../initError';
import Home from '../../routes/home';
// import CompleteInfo from '../../routes/completeInformation';
// import SelectAddress from '../../routes/selectAddress';
// import SelectLocation from '../../routes/selectLocation';
// import Review from './../../routes/review';


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
                    {/* <Route exact path="/contactus" component={ContactUs} />
                   
                    <Route exact path="/tempbasket/:basketId?" component={TempBasket} />
                    <Route path="/completeInformation" component={CompleteInfo} />
                    <Route path="/selectAddress" component={SelectAddress} />
                    <Route path="/selectLocation" component={SelectLocation} />
                    <Route path="/review" component={Review} />
                    <Route path="/afterGateway/:status/:transId" component={AfterGateway} /> */}
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
