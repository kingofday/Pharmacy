import 'bootstrap/dist/css/bootstrap.min.css';
import './assets/css/index.css';
import './assets/css/material-design-iconic-font.min.css';
import 'react-toastify/dist/ReactToastify.css';
import React from 'react';
import ReactDOM from 'react-dom';
import 'react-app-polyfill/ie11';
import 'react-app-polyfill/stable';
//import * as serviceWorker from './serviceWorker';

import Layout from './shared/layout/layout';

import { Provider } from 'react-redux';
import { create } from 'jss';
import rtl from 'jss-rtl';
import { MuiThemeProvider, createMuiTheme, StylesProvider, jssPreset } from '@material-ui/core/styles';
import { PersistGate } from 'redux-persist/integration/react'
import persistStore from './redux/persistStore'
const { store, persistor } = persistStore();
const theme = createMuiTheme({
    direction: 'rtl',
});

// Configure JSS
const jss = create({ plugins: [...jssPreset().plugins, rtl()] });

function RTL(props) {
    return (
        <StylesProvider jss={jss}>
            {props.children}
        </StylesProvider>
    );
}
ReactDOM.render(
    <Provider store={store}>
        <RTL>
            <MuiThemeProvider theme={theme}>
                <PersistGate loading={null} persistor={persistor}>
                    <Layout />
                </PersistGate>
            </MuiThemeProvider>

        </RTL>
    </Provider>, document.getElementById('root'));

//serviceWorker.register();