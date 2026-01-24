from flask import Flask, request

app = Flask(__name__)

@app.route("/hello/", methods=['GET'], defaults={'name': 'No Name'})
def hello_world(name):
    name =  request.args.get('name')
    return {'message': f'Hello, World! {name}'}

if __name__ == '__main__':
    app.run(debug=True, port=5000)