// API logging functions for debugging
window.logApiCall = (method, url, data) => {
    console.log(`🚀 API Call: ${method} ${url}`);
    if (data) {
        console.log('📤 Request Data:', JSON.parse(data));
    }
};

window.logApiResponse = (statusCode, data) => {
    if (statusCode >= 200 && statusCode < 300) {
        console.log(`✅ API Response (${statusCode}):`, JSON.parse(data));
    } else {
        console.log(`❌ API Error (${statusCode}):`, JSON.parse(data));
    }
};
