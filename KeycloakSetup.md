# Setup Keycloak

## Adjust jwt mapping
In order to get authorization to work with Keycloak, you will need to add a new role to Client Scopes.

- Login to Keycloak Admin page
- Select the realm you want to use
- Goto Client Scopes
- Goto 'roles' entry
- Goto Mappers
- Click Add mapper -> By configuration -> User Client Role
- Give the new role a name
- Multivated must be on
- Token claim name must be 'role'
- Add to access token must be on

[Source](https://github.com/tuxiem/AspNetCore-keycloak?tab=readme-ov-file#keycloak-configuration)

## Enable Direct Access Grants
In order to get jwt token from Keycloak via curl directly, you need to change a setting in the Keycloak admin console.
- Select the realm you want to use
- Clients 
- ``customer-api``
- Settings Tab
- Capability config
- Direct Access Grants Enabled: ON

## Create 

# How to obtain a jwt from KeyCloak
```bash	
curl -d 'client_id=customer-api' -d 'username=admin@tributech.io' -d 'password=changeme' -d 'grant_type=password' 'http://localhost:8085/realms/customer/protocol/openid-connect/token'
```
response will be like this
```json
{
    "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJpN01DOGtqTnVvTzRJazB6ZEU5WF9CZWYwUk1fcGtXSEVHV01DUlBTQzdzIn0.eyJleHAiOjE3MTUzNTc3MzgsImlhdCI6MTcxNTM1NzQzOCwianRpIjoiMDVmZTE2NjctOTkzYy00MDRkLThjOGYtMDlkNzkwYjIzMmRmIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDg1L3JlYWxtcy9jdXN0b21lciIsImF1ZCI6WyJjdXN0b21lci1hcGkiLCJyZWFsbS1tYW5hZ2VtZW50Il0sInN1YiI6IjEyYzVmNDg5LTAyZGQtNGY5MS1hYWM5LWRlMTIxYjYzM2ZlOCIsInR5cCI6IkJlYXJlciIsImF6cCI6ImN1c3RvbWVyLWFwaSIsInNlc3Npb25fc3RhdGUiOiJhODdjOWNjMi0zYmIyLTRlNWEtYmJlYi0wMjlhMzhhMmNjNDYiLCJhY3IiOiIxIiwiYWxsb3dlZC1vcmlnaW5zIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTA4MCIsImh0dHBzOi8vbG9jYWxob3N0OjU0NDMiXSwicmVzb3VyY2VfYWNjZXNzIjp7InJlYWxtLW1hbmFnZW1lbnQiOnsicm9sZXMiOlsibWFuYWdlLXVzZXJzIiwidmlldy11c2VycyIsInF1ZXJ5LWdyb3VwcyIsInF1ZXJ5LXVzZXJzIl19LCJjdXN0b21lci1hcGkiOnsicm9sZXMiOlsiQWRtaW4iXX19LCJzY29wZSI6ImVtYWlsIHByb2ZpbGUiLCJzaWQiOiJhODdjOWNjMi0zYmIyLTRlNWEtYmJlYi0wMjlhMzhhMmNjNDYiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwicm9sZSI6WyJBZG1pbiJdLCJuYW1lIjoiQWRtaW4gQ3VzdG9tZXIiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJhZG1pbkB0cmlidXRlY2guaW8iLCJnaXZlbl9uYW1lIjoiQWRtaW4iLCJmYW1pbHlfbmFtZSI6IkN1c3RvbWVyIiwiZW1haWwiOiJhZG1pbkB0cmlidXRlY2guaW8ifQ.qC_8UWSmrfZyNElQLNejJ3LqKd4Ghz0JEQ2qkQoyspYIoSmBXjPBQpXbQU5ZcVsTdKJxAc6f98BOYkM4mwxGS4x1CeR1K9W_vJAMmdqPUJmhxdnTitxzP2w4yw_Kd6e94gq6Z23MfX9uZA5jfALdWJWy6U4ttxLiM9CtxPxIxqTOlO20Ltg0THY7HwPEHM75ZLBbkVH6aGm_y9fQ1rV0TKEMMyTAdfvUy5PNqLw5TgLwvwUx6k0GpO-cZRU675SIkpSYmvjZ3LZuQsOhcMuE-GTtjRbDymKY983e5rRrQf5fmEim_Ve0g-aUeulydkeBzLsJl_bTpi9yEmezdV2vQA",
    "expires_in": 300,
    "refresh_expires_in": 1800,
    "refresh_token": "eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICI2MTA5OTkwMi1mMDU3LTQ4MGItYTNmZS1jMTJlMTllNmJmNmYifQ.eyJleHAiOjE3MTUzNTkyMzgsImlhdCI6MTcxNTM1NzQzOCwianRpIjoiYTE2YzU0YTAtODU3Yi00ZGUwLTkzODAtYjk3MjI4ZWJiMmQ5IiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDg1L3JlYWxtcy9jdXN0b21lciIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6ODA4NS9yZWFsbXMvY3VzdG9tZXIiLCJzdWIiOiIxMmM1ZjQ4OS0wMmRkLTRmOTEtYWFjOS1kZTEyMWI2MzNmZTgiLCJ0eXAiOiJSZWZyZXNoIiwiYXpwIjoiY3VzdG9tZXItYXBpIiwic2Vzc2lvbl9zdGF0ZSI6ImE4N2M5Y2MyLTNiYjItNGU1YS1iYmViLTAyOWEzOGEyY2M0NiIsInNjb3BlIjoiZW1haWwgcHJvZmlsZSIsInNpZCI6ImE4N2M5Y2MyLTNiYjItNGU1YS1iYmViLTAyOWEzOGEyY2M0NiJ9.uE17j9eMU02z0b6jTO7S8cq1jN2Vn3tRZSRFs8ED_s8",
    "token_type": "Bearer",
    "not-before-policy": 0,
    "session_state": "a87c9cc2-3bb2-4e5a-bbeb-029a38a2cc46",
    "scope": "email profile"
}
```
Copy the access_token and use it in the Authorization header in your requests to the API.