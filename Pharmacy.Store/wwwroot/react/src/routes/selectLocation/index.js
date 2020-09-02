import React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router-dom';
import CustomMap from '../../shared/map';
import { SetLocationAction } from './../../redux/actions/mapAction';
import strings from './../../shared/constant';
import queryString from 'query-string'
import Button from './../../shared/Button';

class SelectLocation extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: ''
        };
        const values = queryString.parse(this.props.location.search)

        if (values['lng']) this.lng = parseFloat(values['lng'])
        else this.lng = null;
        if (values['lat']) this.lat = parseFloat(values['lat'])
        else this.lat = null;
    }

    _mapChanged(lng, lat) {
        this.lng = lng;
        this.lat = lat;
    }

    _setLocation() {
        this.props.setLocation(this.lng, this.lat);
        this.setState(p => ({ ...p, redirect: '/selectAddress' }));
        //this.props.history.goBack();
    }
    render() {
        if (this.state.redirect)
            return <Redirect to={this.state.redirect} />
        return (
            <div className="page-select-location">
                <CustomMap height='100vh' lng={this.lng} lat={this.lat} onChanged={this._mapChanged.bind(this)} />
                <Button className='btn-next' onClick={this._setLocation.bind(this)}>{strings.selectLocation}</Button>
            </div>
        );
    }

}

// const mapStateToProps = state => {
//     return { ...state.basketReducer };
// }

const mapDispatchToProps = dispatch => ({
    setLocation: (lat, lng) => dispatch(SetLocationAction(lat, lng))
});

export default connect(null, mapDispatchToProps)(SelectLocation);
