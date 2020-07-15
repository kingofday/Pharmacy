import React, { Component } from 'react';
import Strings from './../../shared/constant';

class NotFound extends Component {
    componentDidMount(){
       // window.location.href = "https://about.Pharmacy.me/contact/";
    }
    render() {
        console.log('not found');
        return (
            <div className="not-found" style={{ textAlign: 'center' }}>
                <p className="text-center" style={{ padding: '50px' }}>{Strings.pleasWait}...</p>
            </div>

        );
    };

}

export default NotFound;