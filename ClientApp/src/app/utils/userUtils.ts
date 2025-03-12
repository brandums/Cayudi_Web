export class userUtils {
 
  static getUserInfo() {
    const token = this.getToken();
    let payload;
    try {

      if (token) {
        payload = token.split(".")[1];
        payload = window.atob(payload);
        const userInfo = JSON.parse(payload);
        userInfo.id = Number(userInfo.id)
        return userInfo;
      } else {
        return null;
      }
    }
    catch (error)
    {
      return null;
    }
  }

  private static getToken() {
    return localStorage.getItem("jwtToken");
  }
}
