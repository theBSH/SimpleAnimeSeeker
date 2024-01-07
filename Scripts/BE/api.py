import requests
from datetime import datetime

api_endpoint = 'https://kitsu.io/api/edge/anime'


def search(inp):
    res = []

    search_query = inp

    params = {
        'filter[text]': search_query,
    }

    response = requests.get(api_endpoint, params=params)

    if response.status_code == 200:
    
        data = response.json()
        
        for anime in data['data']:
            temp = []
            temp.append(int(anime['id']))
            if 'attributes' in anime and 'en' in anime['attributes']['titles']:
               temp.append(anime['attributes']['titles']['en'])
            else:
               temp.append('N/A')
            temp.append(datetime.strptime((anime['attributes']['createdAt']),'%Y-%m-%dT%H:%M:%S.%fZ').year)
            temp.append(anime['attributes']['posterImage']['tiny'])
            temp.append(anime['attributes']['posterImage']['small'])
            temp.append(anime['attributes']['synopsis'])
            temp.append(anime['attributes']['ageRating'])
            res.append(temp)
    else:
        print(f"Error: {response.status_code}, {response.text}")
    return res