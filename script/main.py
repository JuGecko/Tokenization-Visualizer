# import spacy
# from transformers import AutoTokenizer
#
# def tokenize_text(text, model_type):
#     if model_type == 'spacy_en':
#         nlp = spacy.load('en_core_web_sm')
#         return [token.text for token in nlp(text)]
#     elif model_type == 'bert':
#         tokenizer = AutoTokenizer.from_pretrained('bert-base-uncased')
#         return tokenizer.tokenize(text)
#     else:
#         raise ValueError("Unknown model type")
#
# print(tokenize_text("This is a test.", "bert"))

from fastapi import FastAPI
from pydantic import BaseModel
from transformers import AutoTokenizer

app = FastAPI()

# You can preload multiple models if you want
tokenizers = {
    "bert": AutoTokenizer.from_pretrained("bert-base-uncased"),
    "roberta": AutoTokenizer.from_pretrained("roberta-base"),
    "gpt2": AutoTokenizer.from_pretrained("gpt2"),
}

class TokenRequest(BaseModel):
    Text: str
    Model: str  # e.g., "bert", "roberta", "gpt2"

@app.post("/tokenize")
def tokenize(req: TokenRequest):
    model = req.Model.lower()
    if model not in tokenizers:
        return {"error": "Unsupported model. Choose from: bert, roberta, gpt2"}

    tokenizer = tokenizers[model]
    tokens = tokenizer.tokenize(req.Text)
    # Ensure tokens are strings and use the correct property name
    tokens_list = [str(token) for token in tokens]
    return {"Tokens": tokens_list}
