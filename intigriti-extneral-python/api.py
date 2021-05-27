import requests
import json
from oauthlib.oauth2 import BackendApplicationClient
from requests_oauthlib import OAuth2Session

client_id = input('Client ID: ')
client_secret = input('Client Secret: ')

client = BackendApplicationClient(client_id=client_id)
oauth = OAuth2Session(client=client)
token_response = oauth.fetch_token(token_url='https://login.intigriti.com/connect/token', client_id=client_id, client_secret=client_secret)
access_token = token_response.get('access_token')

api_response = requests.get('https://api.intigriti.com/external/v1.2/submissions', headers={'Authorization': f'Bearer {access_token}'}).json()
print(json.dumps(api_response, indent = 2))