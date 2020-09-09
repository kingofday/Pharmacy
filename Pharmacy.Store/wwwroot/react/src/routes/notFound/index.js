import React, { Component } from 'react';
import Strings from './../../shared/constant';
import notFoundImage from './../../assets/images/img-404.png';

class NotFound extends Component {
    render() {
        return (
            <div className="not-found d-flex flex-column justify-content-center align-items-center page-comp">
                <img src={notFoundImage} alt="not found" style={{ width: '300px',maxWidth:'100%',margin:'15px' }} />
                <h5 className="text-center">{Strings.notFound} !!!</h5>
            </div>

        );
    };

}

export default NotFound;