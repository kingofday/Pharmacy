import React from 'react';
import Modal from '../modal';
import { connect } from 'react-redux'
import Store from '../../routes/store';
import ContactUs from '../../routes/contactUs';
import Product from '../../routes/product';
import Basket from './../../routes/basket';
import TempBasket from './../../routes/tempBasket';
import AfterGateway from '../../routes/afterGateway';
import NotFound from '../../routes/notFound';
import InitError from '../../shared/initError';
import { ToastContainer } from 'react-toastify';
import CompleteInfo from '../../routes/completeInformation';
import SelectAddress from '../../routes/selectAddress';
import SelectLocation from '../../routes/selectLocation';
import Review from './../../routes/review';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';

class Layout extends React.Component {
    render() {
        return (
            <Router className="layout">
                {/* <CustomHeader /> */}
                <Switch>
                    <Route exact path="/store/:id" component={Store} />
                    <Route exact path="/product/:id" component={Product} />
                    <Route exact path="/contactus" component={ContactUs} />
                    <Route exact path="/basket" component={Basket} />
                    <Route exact path="/tempbasket/:basketId?" component={TempBasket} />
                    <Route path="/completeInformation" component={CompleteInfo} />
                    <Route path="/selectAddress" component={SelectAddress} />
                    <Route path="/selectLocation" component={SelectLocation} />
                    <Route path="/review" component={Review} />
                    <Route path="/afterGateway/:status/:transId" component={AfterGateway} />
                    <Route path="/:msg?" component={NotFound} />
                </Switch>
                <Modal />
                <InitError />
                <ToastContainer containerId={'common_toast'} rtl />
            </Router>
        );
    }
}

export default connect(null, null)(Layout);
