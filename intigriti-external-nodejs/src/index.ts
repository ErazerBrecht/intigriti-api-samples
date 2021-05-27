import readline from 'readline-promise';
import { Issuer } from 'openid-client';
import got, { OptionsOfTextResponseBody } from 'got';

const std = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

const ask = (text: string) => std.questionAsync(text);
const run = async () => {
    const clientId = await ask('Client ID: ');
    const clientSecret = await ask('Client SECRET: ');

    const issuer = await Issuer.discover('https://login.intigriti.com');
    const client = new issuer.Client({ client_id: clientId, client_secret: clientSecret });
    const tokens = await client.grant({ grant_type: "client_credentials" });

    const url = "https://api.intigriti.com/external/v1.2/submissions";
    const options: OptionsOfTextResponseBody = { headers: { "Authorization": `Bearer ${tokens.access_token}` } };
    const resp = await got.get(url, options);
    console.log(JSON.stringify(JSON.parse(resp.body), null, 2));
}

run().then(() => {
    console.log('Donzo');
    process.exit();
    }, err => {
    console.error('Something went wrong!');
    console.error(err);
    process.exit();
});
