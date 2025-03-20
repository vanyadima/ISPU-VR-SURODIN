import os

def split_file(file_path, chunk_size=45*1024*1024):
    with open(file_path, 'rb') as f:
        chunk_number = 0
        while True:
            chunk = f.read(chunk_size)
            if not chunk:
                break
            chunk_filename = f"{file_path}.part{chunk_number}"
            with open(chunk_filename, 'wb') as chunk_file:
                chunk_file.write(chunk)
            chunk_number += 1

def process_directory(directory):
    for root, _, files in os.walk(directory):
        for file in files:
            file_path = os.path.join(root, file)
            if os.path.getsize(file_path) > 100*1024*1024:
                split_file(file_path)
                os.remove(file_path)

if __name__ == "__main__":
    process_directory(os.getcwd())
