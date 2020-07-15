import React, { Component } from 'react'
import Mapir from 'mapir-react-component';
import markerImage from './../../assets/images/marker.png';

const Map = Mapir.setToken({
  transformRequest: (url) => {
    return {
      url: url,
      headers: {
        'x-api-key': 'eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjQwMjBmN2NmMTIwYWVjZjhiMmQ4OTI2OTZhMjFkNGFiNGRmMDk2OTAxMGQ5ZGEzMTkwNzVjZTRmMTk3OTIwMzc3MzZjMjdhZjk5YWZiMjljIn0.eyJhdWQiOiI4NjA5IiwianRpIjoiNDAyMGY3Y2YxMjBhZWNmOGIyZDg5MjY5NmEyMWQ0YWI0ZGYwOTY5MDEwZDlkYTMxOTA3NWNlNGYxOTc5MjAzNzczNmMyN2FmOTlhZmIyOWMiLCJpYXQiOjE1ODYyNDk0OTksIm5iZiI6MTU4NjI0OTQ5OSwiZXhwIjoxNTg4ODQxNDk5LCJzdWIiOiIiLCJzY29wZXMiOlsiYmFzaWMiXX0.OyA_WsGXMkyoZDvf7xBSHHrqD7aEzeMV71Mibp1gFOqNm2MrRJBqm1QBg81bMyq6RJCW2giWJS9ZDU0m0DPyaRan1-fE8RQ7qI-cFbr-KUCzbDzfHuYSWz4ndn-TF4AIW9xW5gvwXOIGGmAoXz5etw0FEYqFJ5UobOLigpM6bMvHjDDwpPHHro8eCp2CTPZYMZFZuPwAGI0Wpz51m2fUnYe0R15gwZEg3M9p5MLXnXa3IcP78P_cpaepdDQ11Jz9CFqib1tj5gQzc9Xry-PLuB1vXx2FOleSHiRJkyPgoFUa-v0bNfimpZq2x_VeNdesOnWCxcWTfPpUjpFaZYGCnw', //Mapir api key     
        'Mapir-SDK': 'reactjs'
      },
    }
  }
});

export default class CustomMap extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      coords: {
        lng: this.props.location ? this.props.location.lng : 51.337848,
        lat: this.props.location ? this.props.location.lat : 35.699858
      },
      zoom: 13
    }
  }

  _onClick(p) {
    this.setState(prev => ({
      ...prev,
      coords: {
        lng: p.lng,
        lat: p.lat
      },
    }));
    if (this.props.onChange)
      if (this.props.onChanged)
        this.props.onChanged(p.lng, p.lat);
  }

  _setLocation(lng, lat) {
    this._onClick({ lng: lng, lat: lat });
  }

  _setUserLocation = (p) => {
    if (!navigator.geolocation)
      return;
    navigator.geolocation.getCurrentPosition(position => {
      this.setState(p => ({
        ...p,
        coords: {
          lng: position.coords.longitude,
          lat: position.coords.latitude
        },
      }));
    });
  };
  componentDidMount() {
    //this._setUserLocation();
    this.setState(p => ({ ...p, loading: false }));
    console.log(this.props.location);
  }
  render() {
    return (
      <Mapir
        width='100%'
        style={{ width: '100px' }}
        onClick={(e, p) => this._onClick(p.lngLat)}
        center={[this.state.coords.lng, this.state.coords.lat]}
        movingMethod='flyTo'
        minZoom={[13]}
        Map={Map}>
        <Mapir.Marker
          coordinates={[this.state.coords.lng, this.state.coords.lat]}
          Image={markerImage}
          anchor="bottom"> </Mapir.Marker>
      </Mapir>
    );
  }
}