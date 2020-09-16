import strings from './../shared/constant';
import React from 'react'
import { toast } from 'react-toastify';

export function arrangeInRows(columnCount, items, wrapperClassName) {
    let rows = [];
    for (let i = 0; i < items.length; i += columnCount) {
        rows.push(<div key={i} className='arraned-row'>{items.slice(i, i + columnCount)}</div>)
    }
    return (<div className={wrapperClassName}>
        {rows}
    </div>);

}
export function checkLocalStorage() {
    if (!localStorage) toast(strings.useModernBrowser, { type: toast.TYPE.INFO });
}

export function commaThousondSeperator(input) {
    let str = isNaN(input) ? input : input.toString();
    return str.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};

export function getlocalStorageSizes() {
    var total = 0;
    for (var x in localStorage) {
        var amount = (localStorage[x].length * 2) / 1024 / 1024;
        total += amount;
        console.log(x + " = " + amount.toFixed(2) + " MB");
    }
    console.log("Total: " + total.toFixed(2) + " MB");
}

export function getCurrentLocation() {

    return new Promise((resolve) => {
        if (!navigator.geolocation)
            resolve(null);
        navigator.geolocation.getCurrentPosition(function (position) {
            resolve({
                lng: position.coords.longitude,
                lat: position.coords.latitude
            });
        }, function (error) {
            resolve(null);
        }, { enableHighAccuracy: false, maximumAge: 15000, timeout: 30000 });
    });
}

export const validate = {
    mobileNumber: function (mobNumber) {
        if (isNaN(mobNumber)) return false;
        else if (!new RegExp(/^9\d{9}$/g).test(mobNumber)) return false;
        else return true;
    },
    email: function (email) {
        const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }
};

export function cacheData(key, data) {
    if ('caches' in window) {
        caches.open('pharma-pwa-1').then(function (cache) {
            cache.put(key, data);
            return true;
        });
    }
    return false;
}

export function toDataURL(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        var reader = new FileReader();
        reader.onloadend = function () {
            callback(reader.result);
        }
        reader.readAsDataURL(xhr.response);
    };
    xhr.open('GET', url);
    xhr.responseType = 'blob';
    xhr.send();
}
